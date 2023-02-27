using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreSite1.Data;
using CoreSite1.Models;
using SampleApp.Utilities;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Product
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class CreateModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        private readonly string _targetFilePath;

        //[BindProperty]
        //public BufferedSingleFileUploadPhysical FileUpload { get; set; }

        private readonly IHostingEnvironment _HostEnvironment;

        public string path = "";

        public CreateModel(CoreSite1.Data.ApplicationDbContext context,
       IConfiguration config, IHostingEnvironment HostEnvironment)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");

            // To save physical files to a path provided by configuration:
            _targetFilePath = config.GetValue<string>("StoredFilesPath");

            // To save physical files to the temporary files folder, use:
            //_targetFilePath = Path.GetTempPath();

            //hosting environement path
            _HostEnvironment = HostEnvironment;

            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Categorys, "CategoryId", "CategoryId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Brand");
            //to locate ~/wwwroot/CSS
            string webRootPath = _HostEnvironment.WebRootPath;
            string contentRootPath = _HostEnvironment.ContentRootPath;


            path = Path.Combine(webRootPath, "images\\FASHION_DATA");
            //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
            return Page();
        }

        [BindProperty]
        public CoreSite1.Models.Product Product { get; set; }
        [BindProperty]
        public CoreSite1.Models.Variant Variant { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    //Result = "Please correct the form.";

            //    return Page();
            //}

            //var formFileContent =
            //    await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
            //        FileUpload.FormFile, ModelState, _permittedExtensions,
            //        _fileSizeLimit);

          
            ////////////////////////////////////
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Product.AddedDate = DateTime.Now;
            Product.AddedBy = User.Identity.Name;

            _context.Products.Add(Product);
            
            Variant.ProductId = Product.ProductId;
            Variant.OptionalPrice = Product.Price;
            Variant.AddedDate = DateTime.Now;
            Variant.AddedBy = User.Identity.Name;
            Variant.IsDefaulProduct = true;
            Variant.Name = Product.Title;
            Variant.OptionalImageURL = Product.ProductArtUrl;

            _context.Variants.Add(Variant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
    public class BufferedSingleFileUploadPhysical
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string Note { get; set; }
    }
}