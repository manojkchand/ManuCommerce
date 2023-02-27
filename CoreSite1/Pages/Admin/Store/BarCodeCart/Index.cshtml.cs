using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Models;
using CoreSite1.ViewModels;

namespace CoreSite1.Pages.Admin.BarCodeCart
{
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        //public IList<CoreSite1.Models.Category> Category { get; set; }
        public ShoppingCartViewModel viewModel { get; set; }

        public async Task OnGetAsync(int? pid, int? vid, long? barcode)
        {
            int id = 0;
           
            if (pid != null) { 
                id = (int)pid;
            }

            //Category = await _context.Categorys.ToListAsync();
            var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);

            if (barcode != null)
            {
                var v = _context.Products.AsEnumerable()
                   .Where(p => p.UPC == barcode); //.Where(p => p.UPC == barcode).FirstOrDefault();
                if (v.Count() != 0) { id = v.First().ProductId; }


            }
           
            


            if(id != 0 && id != null) { //|| barcode != null
                //if (barcode != null)
                //{
                //    id = _context.Products
                //       .Single(Product => Product.UPC == barcode).ProductId;

                //}
                if (vid == null)//check if its not variant product
                {
                    // Retrieve the Product from the database
                    var addedProduct = _context.Products
                    .Single(Product => Product.ProductId == id);
                    // Add it to the shopping cart
                    cart.AddToCart(addedProduct);
                    viewModel = new ShoppingCartViewModel
                    {
                        CartTotal = cart.GetTotal(),
                        CartItems = cart.GetCartItems()
                        
                    };
                }
                else//if variant product
                {
                    // Retrieve the Product from the database
                    var addedProduct = _context.Products
                    .Single(Product => Product.ProductId == id);
                    var addedVariant = _context.Variants
                    .Single(Variant => Variant.VariantId == vid);
                    // Add it to the shopping cart
                    cart.AddToCart(addedProduct, addedVariant);
                    viewModel = new ShoppingCartViewModel
                    {
                        CartTotal = cart.GetTotal(),
                        CartItems = cart.GetCartItems()

                    };
                }

            }
            else
            {
                viewModel = new ShoppingCartViewModel
                {
                    CartTotal = cart.GetTotal(),
                    CartItems = cart.GetCartItems()
                    
                };
            }
            // Set up our ViewModel

            // Return the view
            //return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        //public ActionResult AddToCart(int id)
        //{
        //    // Retrieve the Product from the database
        //    var addedProduct = storeDB.Products
        //    .Single(Product => Product.ProductId == id);
        //    // Add it to the shopping cart
        //    var cart = ShoppingCart.GetCart( storeDB, this.HttpContext);
        //    cart.AddToCart(addedProduct);
        //    // Go back to the main store page for more shopping
        //    return RedirectToAction("Index");
        //}
        //
        // GET: /Store/AddToCart/5             /MyStore/ShoppingCart?handler=AddToCart&id=5
        public JsonResult OnGetAddToCart(int id, int? vid)
        {
            ShoppingCartJsonViewModel results;
            if (vid == null)//check if its not variant product
            {
                // Retrieve the Product from the database
                var addedProduct = _context.Products
                .Single(Product => Product.ProductId == id);
                // Add it to the shopping cart
                var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);
                cart.AddToCart(addedProduct);
                // Go back to the main store page for more shopping
                //return RedirectToAction("/MyStore/ShoppingCart");
                //this final price is not check with regards to variant product. If your variiant product price is different than 
                //you need to change the price accoridingly.
                Decimal finalprice =
                (addedProduct.Discount == 0 ? finalprice = addedProduct.Price : (finalprice = addedProduct.Price - Math.Round(addedProduct.Price * (addedProduct.Discount / 100m))));

                results = new ShoppingCartJsonViewModel
                {
                    CartTotal = cart.GetTotal(),
                    //CartItems = cart.GetCartItems(),
                    CartCount = cart.GetCount(),
                    ItemCount = cart.GetCartItems().Where(e => e.ProductId == id).Count(),
                    ItemPrice = finalprice * cart.GetCartItems().Where(e => e.ProductId == id).Count()

                };
            }
            else//if variant product
            {
                // Retrieve the Product from the database
                var addedProduct = _context.Products
                .Single(Product => Product.ProductId == id);
                var addedVariant = _context.Variants
                .Single(Variant => Variant.VariantId == vid);
                // Add it to the shopping cart
                var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);
                cart.AddToCart(addedProduct, addedVariant);
                // Go back to the main store page for more shopping
                //return RedirectToAction("/MyStore/ShoppingCart");
                Decimal finalprice =
                (addedProduct.Discount == 0 ? finalprice = addedProduct.Price : (finalprice = addedProduct.Price - Math.Round(addedProduct.Price * (addedProduct.Discount / 100m))));

                results = new ShoppingCartJsonViewModel
                {
                    CartTotal = cart.GetTotal(),
                    //CartItems = cart.GetCartItems(),
                    CartCount = cart.GetCount(),
                    ItemCount = cart.GetCartItems().Where(e=>e.ProductId == id).First().Count,
                    ItemPrice = finalprice * cart.GetCartItems().Where(e => e.ProductId == id).First().Count

                };
            }
            return new JsonResult(results);
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        //[HttpPost]
        public JsonResult OnGetRemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);
            // Get the name of the Product to display confirmation
            int productid = _context.Carts
            .Single(item => item.RecordId == id).ProductId;
            var product = _context.Products.Where(item => item.ProductId == productid).First();
            string ProductName = product.Title.ToString();
            Decimal finalprice=
            (product.Discount == 0?finalprice = product.Price: (finalprice = product.Price - Math.Round(product.Price * (product.Discount / 100m))));

            
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id, _context);
            // Display the confirmation message

            var results = new ShoppingCartJsonViewModel
            {
                Message = ProductName +
                " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id,
                ItemPrice = finalprice * itemCount
            };
            //return Json(results);
            return new JsonResult(results);
        }
        //
        // GET: /ShoppingCart/CartSummary /MyStore/ShoppingCart?handler=CartSummary
        //[ChildActionOnly]
        public ActionResult OnGetCartSummary()
        {
            var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);
            //ViewData["CartCount"] = cart.GetCount();
            //return PartialView("CartSummary");
            return new JsonResult(cart.GetCount());
        }
    }

    //public class ShoppingCartViewModel
    //{
    //    public List<Cart> CartItems { get; set; }
    //    public decimal CartTotal { get; set; }
    //}

    public class ShoppingCartJsonViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
        public decimal ItemPrice { get; set; }
    }
}