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
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MapStock> MapStock { get;set; }

        public async Task OnGetAsync()
        {
            MapStock = await _context.MapStock.ToListAsync();
        }
    }
}
