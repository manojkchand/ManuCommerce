using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreSite1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AuthImageThumbNailService.Pages
{
    public class IndexModel : PageModel
    {
        protected readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CoreSite1.Models.Product> Product { get; set; }
        [BindProperty]
        public List<string> filePaths { get; set; }


        public async Task OnGetAsync()
        {
            Product = await _context.Products.ToListAsync();
            filePaths = new List<string>();
            foreach (var v in Product)
            {
                string s = System.IO.Path.GetDirectoryName(v.ProductArtUrl);
                filePaths.Add(s);
            }

        }

        [BindProperty]
        public List<System.IO.FileInfo> list { get; set; }

        private string Path { get; set; }
        private string ThumbnailPath { get; set; }

        public IActionResult OnPost(IFormCollection values)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Product = _context.Products.Distinct().ToList();
            filePaths = new List<string>();
            foreach (var v in Product)
            {
                string s = System.IO.Path.GetDirectoryName(v.ProductArtUrl);
                filePaths.Add(s);
            }
            foreach (var v in filePaths.Distinct())
            {
                Path = "wwwroot" + v + "\\";
                ThumbnailPath = "wwwroot" + v + "\\thumbnail\\";

                Path = Path.Replace("\\", "/").Replace("\r\n","");
                ThumbnailPath = ThumbnailPath.Replace("\\", "/").Replace("\r\n", "");
                //Path = "wwwroot/Images_thumb/flyweight-images/";
                //ThumbnailPath = "wwwroot/Images_thumb/flyweight-images/thumbnail/";
                if (!Directory.Exists(ThumbnailPath))
                {
                    Directory.CreateDirectory(ThumbnailPath);
                }
                Flyweight.ProgramMainClient client = new Flyweight.ProgramMainClient();
                client.LoadGroups(Path);
                client.CreateGroups(ThumbnailPath);
                //ListImages();
                
            }

            return RedirectToPage("./Index");
        }

        private void ListImages()
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo("wwwroot/Images_thumb/flyweight-images/thumbnail/");
            System.IO.FileInfo[] file = dir.GetFiles();


            if (!file.Any())
            {
                foreach (System.IO.FileInfo file2 in file)
                {
                    if (file2.Extension == ".jpg" || file2.Extension == ".jpeg" || file2.Extension == ".gif" || file2.Extension == ".png")
                    {
                        list.Add(file2);
                        //list.Add(file2.DirectoryName + "/" + file2.Name);
                    }
                }
            }
            // DataList1.DataSource = list;
            // DataList1.DataBind();
        }
    }
}
