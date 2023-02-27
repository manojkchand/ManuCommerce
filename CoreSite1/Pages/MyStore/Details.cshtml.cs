using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Http;

namespace CoreSite1.Pages.MyStore
{
    public class DetailsModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public DetailsModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public CoreSite1.Models.Product Product { get; set; }
        public CoreSite1.Models.Variant Variant { get; set; }

        public List<CoreSite1.Models.Variant> Variants { get; set; }

        [BindProperty]
        public int id { get; set; }

        [BindProperty]
        public int vid { get; set; }

        [BindProperty]
        public int qty { get; set; }
        //public IList<CoreSite1.Models.Category> Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id,int? vid)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product = await _context.Products
               .Include(v => v.Variantlist).FirstOrDefaultAsync(m => m.ProductId == id);


            Variant = await _context.Variants
                .Include(v => v.Product).FirstOrDefaultAsync(m => m.VariantId == vid);

            Variants = _context.Variants
              .Include(v => v.Product).Where(m => m.ProductId == id).ToList();

            if (Variant == null)
            {
                return NotFound();
            }

            //Category = await _context.Categorys.ToListAsync();

            return Page();
        }

         public async Task<IActionResult> OnPost(IFormCollection values)
        {
            //    if (!ModelState.IsValid)
            //    {
            //        return Page();
            //    }
            //    CoreSite1.Models.Product DBProduct = _context.Products.Where(e => e.ProductId == Product.ProductId).FirstOrDefault();
            //    DBProduct.Title = Product.Title;
            //    DBProduct.Price = Product.Price;
            //    DBProduct.Discount = Product.Discount;
            //    DBProduct.StockOfNonVariant = Product.StockOfNonVariant;
            //    DBProduct.Brand = Product.Brand;
            //    DBProduct.ProductArtUrl = Product.ProductArtUrl;
            //    DBProduct.Description = Product.Description;
            //    //DBpage.URL = CreateURL(DBpage);Title Price Discount StockOfNonVariant Brand ProductArtUrl Description

            //    _context.Entry(DBProduct).State = EntityState.Modified;

            //    //_context.Attach(Product).State = EntityState.Modified;

            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ProductExists(Product.ProductId))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            var cart = CoreSite1.Models.ShoppingCart.GetCart(_context, this.HttpContext);


            id = int.Parse(values["id"]);

            vid = int.Parse(values["vid"]);

            qty= int.Parse(values["qty"]);
            //CODE Below Copied from Shopping cart Get method.
            if (vid.ToString() == null)//check if its not variant product
            {
                // Retrieve the Product from the database
                var addedProduct = _context.Products
                .Single(Product => Product.ProductId == id);
                // Add it to the shopping cart
                for (int i = 0; i < qty; i++)
                {
                    cart.AddToCart(addedProduct);
                }
            }
            else//if variant product
            {
                // Retrieve the Product from the database
                var addedProduct = _context.Products
                    .Single(Product => Product.ProductId == id);
                    var addedVariant = _context.Variants
                    .Single(Variant => Variant.VariantId == vid);

                    // Add it to the shopping cart
                    for(int i=0;i<qty;i++)
                    {
                        cart.AddToCart(addedProduct, addedVariant);
                    }

            }




            return Redirect("/MyStore/ShoppingCart");
        }
    }
}
