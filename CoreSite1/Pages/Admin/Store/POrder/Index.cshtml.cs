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

namespace CoreSite1.Pages.POrder
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public IList<CoreSite1.Models.Order> Order { get;set; }

        public string CurrentFilter { get; set; }
        public PaginatedList<CoreSite1.Models.Order> Order { get; set; }

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

            IQueryable<CoreSite1.Models.Order> orderIQ = from s in _context.Orders.Include("OrderDetails").Where(q=>q.OrderType == OrderType.PurchaseOrder)
                                                                  select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString.All(char.IsDigit))
                {
                    orderIQ = orderIQ.Where(s => s.OrderId.Equals(int.Parse(searchString)));
                }
                else
                {
                    orderIQ = orderIQ.Where(s => s.AddedBy.Contains(searchString));
                }

            }

            int pageSize = 10;
            Order = await PaginatedList<CoreSite1.Models.Order>.CreateAsync(
            orderIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            //Order = await _context.Orders
            //    .Include(o => o.Address).Include(o => o.OrderDetails).ToListAsync();
        }
    }
}
