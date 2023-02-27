using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using System.IO;
using CoreSite1.Utilities;
using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace CoreSite1.Pages.Product
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class ExportBulkListModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        private readonly IHostingEnvironment _HostEnvironment;

        [BindProperty]
        public string path { get; set; }

        public ExportBulkListModel(CoreSite1.Data.ApplicationDbContext context, IHostingEnvironment HostEnvironment)
        {
            _context = context;
            //hosting environement path
            _HostEnvironment = HostEnvironment;
            path = "";
        }

        public IList<CoreSite1.Models.Product> Product { get;set; }
        
        public async Task OnGet()
        {
            Product = await _context.Products.ToListAsync();

            String PartPath = "\\Documents\\PickDropFiles\\Export.csv";
            //to locate ~/wwwroot/
            string webRootPath = _HostEnvironment.WebRootPath;
            path = webRootPath + PartPath;
           
        }

        public async Task OnPostAsync()
        {

            Product = await _context.Products.ToListAsync();

            using (var writer = new StreamWriter(path))// "C:\\Users\\L\\source\\repos\\SumanClothing\\CoreSite1\\Pages\\Product\\PickDropFiles\\Export.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(Product);
            }
        }


    }
}
