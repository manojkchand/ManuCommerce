using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Product
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public string NameSort { get; set; }
        //public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        //public string CurrentSort { get; set; }

       // public IList<CoreSite1.Models.Product> Product { get;set; }

        public PaginatedList<CoreSite1.Models.Product> Product { get; set; }

        public async Task OnGetAsync(string currentFilter, string searchString,int? pageIndex)
        {
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<CoreSite1.Models.Product> productIQ = from s in _context.Products
                                                                    select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                productIQ = productIQ.Where(s => s.Title.Contains(searchString)
                || s.Brand.Contains(searchString));
            }

            int pageSize = 10;
            Product = await PaginatedList<CoreSite1.Models.Product>.CreateAsync(
            productIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            //Product = await productIQ.AsNoTracking().ToListAsync();
            //await _context.Products
            //.Include(p => p.Category).ToListAsync();
        }
    }
}
