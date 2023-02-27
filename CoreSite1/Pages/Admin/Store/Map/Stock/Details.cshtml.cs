using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Admin.Store.Map.Stock
{
    public class DetailsModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public DetailsModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public MapStock MapStock { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MapStock = await _context.MapStock.FirstOrDefaultAsync(m => m.MapStockID == id);

            if (MapStock == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
