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
    public class DetailsModel : PageModel
    {
        private readonly ExampleAPI.Data.InternetShopDbContext _context;

        public DetailsModel(ExampleAPI.Data.InternetShopDbContext context)
        {
            _context = context;
        }

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
    }
}
