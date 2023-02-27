using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Variant
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class CreateModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public CreateModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Brand");
            return Page();
        }

        [BindProperty]
        public CoreSite1.Models.Variant Variant { get; set; }
        public CoreSite1.Models.Product Product { get; set; }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            /////////////////////////
            Product = _context.Products.Where(e=>e.ProductId == id).First();
            //Variant = new CoreSite1.Models.Variant();


            
            
                
                Variant.ProductId = Product.ProductId;
                Variant.OptionalPrice = Product.Price;
                Variant.AddedDate = DateTime.Now;
                Variant.AddedBy = User.Identity.Name;
                Variant.IsDefaulProduct = false;
                Variant.Name = Product.Title;
                //Variant.OptionalImageURL = Product.ProductArtUrl;
                //Variant.UnitInStock = 50;
                //Variant.Size = "default";

                //_context.Variants.Add(Variant);
                //_context.SaveChanges();
                

           







            _context.Variants.Add(Variant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}