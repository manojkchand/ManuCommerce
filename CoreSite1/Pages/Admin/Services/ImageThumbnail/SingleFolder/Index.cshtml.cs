using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreSite1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ImageThumbNailService.Pages
{
    public class IndexModel : PageModel
    {
        protected readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CoreSite1.Models.Product> Product { get; set; }

        public async Task OnGetAsync()
        {
                Product = await _context.Products.ToListAsync();
        }

        [BindProperty]
        public List<System.IO.FileInfo> list { get; set; }
        [BindProperty]
        public string Path { get; set; }
        private string ThumbnailPath { get; set; }

        public IActionResult OnPost(IFormCollection values)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Path = "wwwroot/Images_thumb/flyweight-images/";
            ThumbnailPath = Path + "thumbnail/";
            if (!Directory.Exists(ThumbnailPath)) { 
                Directory.CreateDirectory(ThumbnailPath);
            }
            Flyweight.ProgramMainClient client = new Flyweight.ProgramMainClient();
            client.LoadGroups(Path);
            client.CreateGroups(ThumbnailPath);
            //ListImages();

            return RedirectToPage("./Index");
        }

        private void ListImages()
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo("wwwroot/Images_thumb/flyweight-images/thumbnail/");
            System.IO.FileInfo[] file = dir.GetFiles();


            if (!file.Any()) { 
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
