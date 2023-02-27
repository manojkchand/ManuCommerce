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
    public class ProductBEPModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public ProductBEPModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public IList<CoreSite1.Models.Order> Order { get;set; }
        [BindProperty]
        public int stock { get; set; }
        [BindProperty]
        public decimal salesPrice { get; set; }
        [BindProperty]
        public int discount { get; set; }
        [BindProperty]
        public decimal salesTotal { get; set; }
        [BindProperty]
        public int purchasedStock { get; set; }
        [BindProperty]
        public decimal CostPrice { get; set; }
        [BindProperty]
        public decimal CostTotal { get; set; }



        public string CurrentFilter { get; set; }
        public PaginatedList<CoreSite1.Models.Order> Order { get; set; }

        public async Task OnGetAsync(string currentFilter, string searchString, int? pageIndex, int? id,int? stok, decimal? pr, int? dis)
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

            stock = (int)stok;
            salesPrice = (decimal)pr;
            discount = (int)dis;


            IQueryable<CoreSite1.Models.Order> orderIQ = from s in _context.Orders.Include("OrderDetails").Where(q=>q.OrderType == OrderType.PurchaseOrder)
                                                                  select s;


          
            orderIQ = orderIQ.Where(r => r.OrderDetails.Any(rc => rc.ProductId == id));

            //Purchase details are featched from Purchase OrderItems.
            //////////////
            purchasedStock= orderIQ.OrderByDescending(e => e.OrderDate).FirstOrDefault().OrderDetails.Where(p=>p.ProductId == id).FirstOrDefault().Quantity;
            CostPrice = orderIQ.OrderByDescending(e => e.OrderDate).FirstOrDefault().OrderDetails.Where(p => p.ProductId == id).FirstOrDefault().FinalUnitPrice;

            CostTotal = purchasedStock * CostPrice;


            //assumed discounted price
            //this can be calculated currectly from OrderDetails-SpecficProdcu-Adding all FinalUnitPrices. from the date of Purchase Order.
            
            ////var discoutedPrice = discount == 0 ? salesPrice : (salesPrice - Math.Round(salesPrice * (discount / 100m), 2, MidpointRounding.ToEven));

            ////var soldStock = purchasedStock - stock;
            ////salesTotal = discoutedPrice * soldStock;

            //doing the other way.
            IQueryable<CoreSite1.Models.OrderItem> soldorders = _context.OrderItem.Where(p => p.ProductId == id && p.Order.OrderType == OrderType.SalesOrder).Where(h => h.AddedDate > orderIQ.OrderByDescending(e=>e.OrderDate ).FirstOrDefault().OrderDetails.Where(p => p.ProductId == id).FirstOrDefault().AddedDate);
            //.Select(e=>e.FinalUnitPrice).Sum();
            
            foreach (var v in soldorders)
            {
                var temp= v.FinalUnitPrice * v.Quantity;
                salesTotal = salesTotal + temp;
            }
            //salesTotal = soldorders;
            /////////////

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
