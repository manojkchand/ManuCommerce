using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class Dashboard3Model : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public Dashboard3Model(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.OrderItem> OrderItem { get; set; }
        public IList<CoreSite1.Models.Product> Product { get; set; }


        public async Task OnGetAsyn()
        {
            OrderItem = await _context.OrderItem
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Include(o => o.Variant).ToListAsync();

            Product= await _context.Products
               .ToListAsync();
        }


        ////// GET: OrderDetails
        //public JsonResult OnGetProducts()
        //{
        //    var query = _context.Products.TakeLast(5);//from c in _context.OrderItem.Where(m => m.Order.OrderDate.Date == DateTime.Today.Date)

        //    return new JsonResult(query);
        //}

        //public JsonResult OnGetOrders()
        //{
        //    OrderItem = _context.OrderItem.Include(o => o.Order)
        //    .Include(o => o.Product)
        //    .Include(o => o.Variant).ToList();
        //    //var query = _context.OrderItem.include.TakeLast(5);//from c in _context.OrderItem.Where(m => m.Order.OrderDate.Date == DateTime.Today.Date)

        //    return new JsonResult(OrderItem);
        //}

    }
}