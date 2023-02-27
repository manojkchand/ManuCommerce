using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin
{

    [Authorize(Roles = "ThisSiteAdmin")]
    public class ProductEditModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public ProductEditModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CoreSite1.Models.Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products
                .Include(p => p.Category).FirstOrDefaultAsync(m => m.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }
           ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            CoreSite1.Models.Product DBProduct = _context.Products.Where(e => e.ProductId == Product.ProductId).FirstOrDefault();
            DBProduct.Title = Product.Title;
            DBProduct.Price = Product.Price;
            DBProduct.CostPrice = Product.CostPrice;
            DBProduct.Discount = Product.Discount;
            DBProduct.StockOfNonVariant = Product.StockOfNonVariant;
            DBProduct.Brand = Product.Brand;
            DBProduct.ProductArtUrl = Product.ProductArtUrl;
            DBProduct.Description = Product.Description;
            //DBpage.URL = CreateURL(DBpage);Title Price Discount StockOfNonVariant Brand ProductArtUrl Description

            _context.Entry(DBProduct).State = EntityState.Modified;

            //_context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Redirect("/MyStore/details?id=" + Product.ProductId + "&vid="+ _context.Variants.Where(e=>e.ProductId == Product.ProductId && e.IsDefaulProduct == true).FirstOrDefault().VariantId);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
