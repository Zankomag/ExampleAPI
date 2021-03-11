using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExampleAPI.Data;
using ExampleAPI.Data.Models;

namespace ExampleAPI.Web.Pages.OrderItem
{
    public class DeleteModel : PageModel
    {
        private readonly ExampleAPI.Data.InternetShopDbContext _context;

        public DeleteModel(ExampleAPI.Data.InternetShopDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Data.Models.OrderItem OrderItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderItem = await _context.OrderItems.FirstOrDefaultAsync(m => m.Id == id);

            if (OrderItem == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderItem = await _context.OrderItems.FindAsync(id);

            if (OrderItem != null)
            {
                _context.OrderItems.Remove(OrderItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
