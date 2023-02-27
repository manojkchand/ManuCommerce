using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Admin.Store.POrder
{
    public class UpdateStockModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public UpdateStockModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public CoreSite1.Models.Product Product { get; set; }
        public CoreSite1.Models.Variant Variant { get; set; }
        public CoreSite1.Models.Order Order { get; set; }
        [BindProperty]
        public string msg { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id, int? oid, int? qty)
        {
            if (id == null || oid == null)
            {
                return NotFound();
            }
            Order = await _context.Orders.Include(p=>p.OrderDetails).FirstOrDefaultAsync(m => m.OrderId == oid);

            if (Order == null)
            {
                return NotFound();
            }
            //stock = (int)qty;
            //Order = (int)oid;
            if (Order.OrderDetails.Where(p => p.ProductId == id).FirstOrDefault().Status != OrderStatus.Complete)
            {
                Product = await _context.Products
                .Include(p => p.Category).FirstOrDefaultAsync(m => m.ProductId == id);

                Variant = _context.Variants
               .Include(v => v.Product).Where(m => m.ProductId == id && m.IsDefaulProduct == true).FirstOrDefault();
                if (Product == null)
                {
                    return NotFound();
                }
                Variant.OldUnitInStock= Variant.OldUnitInStock + Variant.UnitInStock;
                Variant.UnitInStock = (int)qty;
                Order.OrderDetails.Where(p => p.ProductId == id).FirstOrDefault().Status = OrderStatus.Complete;
                Order.OrderDetails.Where(p=>p.ProductId == id).FirstOrDefault().AddedDate = DateTime.Now;
                //v.UnitInStock--;

                ////update stock in product table if you dont want the variant table at all
                //var v = storeDB.Products.Where(p => p.ProductId == Product.ProductId).First();
                //v.StockOfNonVariant--;


                //// Save changes
                _context.SaveChanges();
                msg = "Stock updated.";
            }
            else { msg = "Stock is already updated for this order.Order status is complete."; }
            return Page();
        }
    }
}
