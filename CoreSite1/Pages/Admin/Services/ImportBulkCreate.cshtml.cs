using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreSite1.Data;
using CoreSite1.Models;
using System.IO;
using System.Globalization;
//using BackgroundTasksSample.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Product
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class ImportBulkCreateModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;
        public IEnumerable<CoreSite1.Models.Product> lines { get; set; }

        private readonly IHostingEnvironment _HostEnvironment;

        [BindProperty]
        public string path { get; set; }
        public ImportBulkCreateModel( CoreSite1.Data.ApplicationDbContext context, IHostingEnvironment HostEnvironment)
        {
            
            _context = context;
            _HostEnvironment = HostEnvironment;
            path = "";
        }
        //public IBackgroundTaskQueue Queue { get; }
        //public IApplicationLifetime _appLifetime { get; }
        //public ILogger<IndexModel> _logger { get; }





        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Brand");

            String PartPath = "\\Documents\\PickDropFiles\\import45.csv";
            //to locate ~/wwwroot/
            string webRootPath = _HostEnvironment.WebRootPath;
            path = webRootPath + PartPath;

            return Page();
        }

        //[BindProperty]
        public List<CoreSite1.Models.Product> Products { get; set; }
        //[BindProperty]
        public List<CoreSite1.Models.Variant> Variants { get; set; }

        [BindProperty]
        public CoreSite1.Models.Product Product { get; set; }
        [BindProperty]
        public CoreSite1.Models.Variant Variant { get; set; }

        //[BindProperty]
        //public string Path { get; set; }

        public IActionResult OnPost()
        {
            var csv = new CsvHelper.CsvReader(new System.IO.StreamReader(path), System.Globalization.CultureInfo.InvariantCulture);
            lines = csv.GetRecords<CoreSite1.Models.Product>();

            Products = new List<Models.Product>();
            Variants = new List<Models.Variant>();



            foreach (var item in lines.ToList())
            {
                //TryUpdateModelAsync(Product);

                Product = new CoreSite1.Models.Product();//{ProductId = productId}
                Product.AddedDate = DateTime.Now;
                Product.AddedBy = User.Identity.Name;
                Product.Title = item.Title;
                Product.Price = item.Price;
                Product.Brand = item.Brand;
                Product.Discount = item.Discount;
                Product.ProductArtUrl = item.ProductArtUrl;
                Product.Description = item.Description;
                Product.StockOfNonVariant = item.StockOfNonVariant;
                Product.CategoryId = item.CategoryId;
                Product.UnitInStock = item.UnitInStock;

                Products.Add(Product);

            }
            _context.Products.AddRange(Products);

            // Save changes
            _context.SaveChanges();


            foreach (var item in Products)
            {
                Variant = new CoreSite1.Models.Variant();//{VariantId = variantId}

                Variant.ProductId = item.ProductId;
                Variant.OptionalPrice = item.Price;
                Variant.AddedDate = DateTime.Now;
                Variant.AddedBy = User.Identity.Name;
                Variant.IsDefaulProduct = true;
                Variant.Name = item.Title;
                Variant.OptionalImageURL = item.ProductArtUrl;
                Variant.UnitInStock = (int)item.UnitInStock;

                Variants.Add(Variant);
            }
            _context.Variants.AddRange(Variants);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        //public IActionResult OnPostAddTask()
        //{
        //    Queue.QueueBackgroundWorkItem(async token =>
        //    {
              
        //    });
        //    return RedirectToPage();
        //}
    }
}