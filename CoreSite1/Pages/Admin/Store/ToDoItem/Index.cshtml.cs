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

namespace CoreSite1.Pages.ToDoItem
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.ToDoItem> ToDoItem { get;set; }

        public async Task OnGetAsync()
        {
            ToDoItem = await _context.ToDoItem.ToListAsync();
        }

        // GET: OrderDetails
        public JsonResult OnGetToDoItem()
        {
            //var applicationDbContext = _context.OrderItems.Include(o => o.Menus).Include(o => o.Order).Select(m => m.Order.OrderDate.Date == DateTime.Today.Date);

            //var v = _context.OrderItems.Select(m => m.Order.OrderDate.Date == DateTime.Today.Date);m.AddedBy == User.Identity.Name && 
            var query = _context.ToDoItem.Where(m=>m.IsCalenderEvent == false).ToListAsync();
                //from c in _context.OrderItem.Where(m => m.Order.OrderDate.Date == DateTime.Today.Date)
                //        group c by c.Product.Title into g
                //        select new
                //        {
                //            MenuName = g.Key,
                //            //MenuName = g.Select(p => p.Menus.Name) ,
                //            NoOfOrder = g.Select(m => m.Quantity).Sum(),
                //            UnitPrice = g.Select(n => n.FinalUnitPrice).Take(1)
                //        };


            return new JsonResult(query);
        }

        // GET: OrderDetails
        public JsonResult OnGetCalenderItem()
        {
            var query = _context.ToDoItem.ToListAsync();


            return new JsonResult(query);
        }
    }
}
