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

namespace CoreSite1.Pages.OrderItem
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public IList<CoreSite1.Models.OrderItem> OrderItem { get;set; }
        public string CurrentFilter { get; set; }
        public PaginatedList<CoreSite1.Models.OrderItem> OrderItem { get; set; }

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

            IQueryable<CoreSite1.Models.OrderItem> orderitemIQ = from s in _context.OrderItem.Include(o => o.Order)
                .Include(o => o.Product)
                .Include(o => o.Variant)
                                                                      select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                if(searchString.All(char.IsDigit))
                {
                    orderitemIQ = orderitemIQ.Where(s => s.OrderId.Equals(int.Parse(searchString)));
                }
                else
                {
                    orderitemIQ = orderitemIQ.Where(s => s.AddedBy.Contains(searchString));
                }
               
            }

            int pageSize = 10;
            OrderItem = await PaginatedList<CoreSite1.Models.OrderItem>.CreateAsync(
            orderitemIQ.AsNoTracking(), pageIndex ?? 1, pageSize);



            //OrderItem = await _context.OrderItem
            //    .Include(o => o.Order)
            //    .Include(o => o.Product)
            //    .Include(o => o.Variant).ToListAsync();
        }
    }
}
