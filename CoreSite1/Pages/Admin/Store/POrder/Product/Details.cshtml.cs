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
using Microsoft.AspNetCore.Http;

namespace CoreSite1.Pages.POrder.Product
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class DetailsModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public DetailsModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public CoreSite1.Models.Product Product { get; set; }
        public CoreSite1.Models.Variant Variant { get; set; }
        public IList<CoreSite1.Models.Variant> Variants { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products
                .Include(p => p.Category).FirstOrDefaultAsync(m => m.ProductId == id);

            Variant = await _context.Variants
               .Include(v => v.Product).Where(v=>v.IsDefaulProduct == true && v.ProductId == id).FirstOrDefaultAsync();

            Variants = await _context.Variants
               .Where(v => v.Product == Product).ToListAsync();

            if (Product == null)
            {
                return NotFound();
            }
            cp = Product.CostPrice;
            return Page();
        }



        [BindProperty]
        public int id { get; set; }

        [BindProperty]
        public int vid { get; set; }

        [BindProperty]
        public int qty { get; set; }

        [BindProperty]
        public decimal cp { get; set; }

        public async Task<IActionResult> OnPost(IFormCollection values)
        {

            var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);


            id = int.Parse(values["id"]);

            vid = int.Parse(values["vid"]);

            qty = int.Parse(values["qty"]);
            //CODE Below Copied from Shopping cart Get method.
            if (vid.ToString() == null)//check if its not variant product
            {
                // Retrieve the Product from the database
                var addedProduct = _context.Products
                .Single(Product => Product.ProductId == id);

                ////change price to cost price for purchase order.
                //addedProduct.Price = decimal.Parse(values["cp"]);
                // Add it to the shopping cart
                for (int i = 0; i < qty; i++)
                {
                    cart.AddToPOCart(addedProduct);
                }
                //doing after addtoPOCart as EF updatedb is triggerd in the call which is updating the product price to CP.
                //change price to cost price for purchase order.
                //addedProduct.Price = decimal.Parse(values["cp"]);
            }
            else//if variant product
            {
                // Retrieve the Product from the database
                var addedProduct = _context.Products
                    .Single(Product => Product.ProductId == id);
                var addedVariant = _context.Variants
                .Single(Variant => Variant.VariantId == vid);

                
                ////change price to cost price for purchase order.
                //addedProduct.Price = decimal.Parse(values["cp"]);
                // Add it to the shopping cart
                for (int i = 0; i < qty; i++)
                {
                    cart.AddToPOCart(addedProduct, addedVariant);
                }
                //doing after addtoPOCart as EF updatedb is triggerd in the call which is updating the product price to CP.
                //change price to cost price for purchase order.
                //addedProduct.Price = decimal.Parse(values["cp"]);
            }
            return Redirect("/Admin/Store/POrder/ShoppingCart");
        }
    }
}
