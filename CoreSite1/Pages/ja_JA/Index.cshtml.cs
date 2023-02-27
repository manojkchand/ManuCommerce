using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSite1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoreSite1.Pages.ja_JA
{
    public class IndexModel : PageModel
    {
        protected readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.PageTemplate> Templates { get; set; }
        public CoreSite1.Models.Page Pages { get; set; }

        //public CoreSite1.Models.Page Page { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pages = await _context.Pages.FirstOrDefaultAsync(m => m.PageId == id);
            Templates = _context.PTemplate.ToList();
            //check if default Language Template is used
            string Turl = Templates.Where(e => e.PageTemplateId == Pages.PageTempleteId).FirstOrDefault().TempleteURL;
            string TurlWithFSlash = Turl + "/";
            string path = this.Url.Action().ToString();// Context.HttpContext.Request.Path.Value;
            if (path != Turl && path != TurlWithFSlash)
            {

                return Redirect(Turl + "?id=" + id);
            }

            if (Pages == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}