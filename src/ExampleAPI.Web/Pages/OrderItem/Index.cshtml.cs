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
    public class IndexModel : PageModel
    {
        private readonly ExampleAPI.Data.InternetShopDbContext _context;

        public IndexModel(ExampleAPI.Data.InternetShopDbContext context)
        {
            _context = context;
        }

        public IList<Data.Models.OrderItem> OrderItem { get;set; }

        public async Task OnGetAsync()
        {
            OrderItem = await _context.OrderItems.ToListAsync();
        }
    }
}
