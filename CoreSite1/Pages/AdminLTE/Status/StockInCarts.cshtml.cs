using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin.Status
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class StockInCartsModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public StockInCartsModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        //public IList<CoreSite1.Models.Cart> cartItems2 { get;set; }
        public IQueryable<CoreSite1.Models.Cart> cartItems { get; set; }
        public void OnGet()
        {
            //var _context = scope.ServiceProvider.GetRequiredService<ManuFashion.Data.ApplicationDbContext>();

            //DbFunctions dbFunctions = null;//required for datediff
            //get all cart items not associated with register user or 2 days old.
            cartItems = from s in _context.Carts//_context.Carts.Where();//all().GroupBy(e => e.ProductId);//.Where(cart => cart.CartId == ShoppingCartId);
                        select s;
            //cartItems2 = from s in cartItems
            //                    group s by s.ProductId
            //                    select s;

            //cartItems = cartItems.GroupBy(e => e.ProductId);
            //foreach (var cartItem in cartItems)
            //{
            //    //log the day time difference
            //    _logger.LogInformation("No of days to since the item added to cart:" + dbFunctions.DateDiffDay(cartItem.DateCreated, DateTime.Now).ToString());

            //    //remove the cart item
            //    _context.Carts.Remove(cartItem);

            //    //increase stock value back as the stock which is decrease while "adding to cart".
            //    //it would have been not required if the shopping cart would have been converted to an order.
            //    if (cartItem.VariantId != null)
            //    {
            //        var v = _context.Variants.Where(p => p.ProductId == cartItem.ProductId && p.VariantId == cartItem.VariantId).First();


            //    }
            //    else
            //    {
            //        var v = _context.Variants.Where(p => p.ProductId == cartItem.ProductId && p.IsDefaulProduct == true).First();
            //         // v.UnitInStock = v.UnitInStock + cartItem.Count;

            //    }


            //}
            //// Save changes
            //_context.SaveChanges();


        }
    }
}