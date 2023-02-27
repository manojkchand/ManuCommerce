using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin.Dashboard
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class TotalPartialModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public TotalPartialModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {

        }

       
        //// GET: Orders Total
        public JsonResult OnGetTotalProduct()
        {
            var v = _context.Products.Count();//.ToList().AsQueryable().Where(m => m.OrderDate.Date == DateTime.Today.Date).Sum(m => m.Total);
            // Array a = new Array[];
            return new JsonResult(v);
        }

        // GET: Orders Total
        public JsonResult OnGetTotalCategory()
        {
            var v = _context.Categorys.Count();//_context.Orders.ToList().AsQueryable().Where(m => m.OrderDate.Date == DateTime.Today.Date).Sum(m => m.Total);
            // Array a = new Array[];
            return new JsonResult(v);
        }

        // GET: Orders Total
        public JsonResult OnGetTotalProductVariant()
        {
            var v = _context.Variants.Count();//_context.Orders.ToList().AsQueryable().Where(m => m.OrderDate.Date == DateTime.Today.Date).Sum(m => m.Total);
            // Array a = new Array[];
            return new JsonResult(v);
        }

        // GET: Orders Total
        public JsonResult OnGetTotalBrands()
        {
            var v = _context.Products.GroupBy(y => y.Brand).Select(y => y.FirstOrDefault()).Count();//.AsQueryable().Where(m => m.OrderDate.Date == DateTime.Today.Date).Sum(m => m.Total);
            // Array a = new Array[]; .GroupBy(y => y.Brand).Select(y => y.FirstOrDefault())
            return new JsonResult(v);
        }
    }
}