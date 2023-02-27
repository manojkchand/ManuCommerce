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

namespace CoreSite1.Pages.Category
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public IList<CoreSite1.Models.Category> Category { get;set; }
        public string CurrentFilter { get; set; }
        public PaginatedList<CoreSite1.Models.Category> Category { get; set; }

        public async Task OnGetAsync(string currentFilter, string searchString, int? pageIndex)
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

            IQueryable<CoreSite1.Models.Category> categoryIQ = from s in _context.Categorys
                                                                    select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                categoryIQ = categoryIQ.Where(s => s.Name.Contains(searchString));
                //|| s.Product.Brand.Contains(searchString));
            }

            int pageSize = 10;
            Category = await PaginatedList<CoreSite1.Models.Category>.CreateAsync(
            categoryIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            //Category = await _context.Categorys.ToListAsync();
        }
    }
}
