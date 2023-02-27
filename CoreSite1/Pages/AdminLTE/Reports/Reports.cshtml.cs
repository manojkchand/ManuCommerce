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

namespace CoreSite1.Pages.Admin.Reports
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class ReportsModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public ReportsModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }
        //public IList<CoreSite1.Models.OrderItem> OrderItem { get;set; }

        //public async Task OnGetAsyn()
        //{
        //    //OrderItem = await _context.OrderItem
        //    //    .Include(o => o.Order)
        //    //    .Include(o => o.Product)
        //    //    .Include(o => o.Variant).ToListAsync();
        //}

        public IList<CoreSite1.Models.OrderItem> OrderDetail { get; set; }
        
        public JsonResult OnGetList()
        {
            List<string> lstString = new List<string>
            {
                "Val 1",
                "Val 2",
                "Val 3"
            };
            return new JsonResult(lstString);
        }

        // GET: Orders Total
        public JsonResult OnGetHourlyOrderTotal()
        {
            var v = _context.Orders.ToList().AsQueryable().GroupBy(m => m.OrderDate.Date.Hour);
            // Array a = new Array[];
            var query = from c in _context.Orders//.Where(m => m.OrderDate.Date == DateTime.Today.Date)
                        group c by c.OrderDate.Hour into g
                        select new
                        {
                            Hour = g.Key,
                            //MenuName = g.Select(p => p.Menus.Name) ,
                            NoOfOrder = g.Count(),
                            //OrderTotal = g.Sum(n => n.UnitPrice)
                        };


            return new JsonResult(query);
        }

        // GET: OrderDetails
        public JsonResult OnGetReporting()
        {
            //var applicationDbContext = _context.OrderItems.Include(o => o.Menus).Include(o => o.Order).Select(m => m.Order.OrderDate.Date == DateTime.Today.Date);

            //var v = _context.OrderItems.Select(m => m.Order.OrderDate.Date == DateTime.Today.Date);
            var query = from c in _context.OrderItem.Where(m => m.Order.OrderDate.Date.Date == DateTime.Today.Date.Date)
                        group c by c.Product.Title into g


                        select new
                        {
                            MenuName = g.Key,
                            //MenuName = g.Select(p => p.Menus.Name) ,
                            NoOfOrder = g.ToList().Sum(m => m.Quantity),
                            UnitPrice = g.ToList().Sum(m => m.FinalUnitPrice)/g.Count()

                        };


            return new JsonResult(query);
        }

        // GET: Orders Total
        public JsonResult OnGetOrderTotal()
        {
            var v = _context.Orders.ToList().AsQueryable().Where(m => m.OrderDate.Date == DateTime.Today.Date).Sum(m => m.Total);
            // Array a = new Array[];
            return new JsonResult(v);
        }
    }
}
