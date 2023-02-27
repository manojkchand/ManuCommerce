using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreSite1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CoreSite1.Pages.Home
{
    public class IndexModel : PageModel
    {
        protected readonly ApplicationDbContext _context;
        protected SignInManager<ExtendedUser> SignInManager;
        protected UserManager<ExtendedUser> UserManager;
        //private readonly SignInManager<ExtendedUser> _signInManager;
        //private readonly UserManager<ExtendedUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ExtendedUser> _userManager,
            SignInManager<ExtendedUser> _signInManager)
        {
            _context = context;
            UserManager = _userManager;
            SignInManager = _signInManager;
        }

        public IList<CoreSite1.Models.Category> Category { get; set; }
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
            if(Pages.LayoutPage == "~/Pages/Shared/_LayoutMyStore.cshtml")
            {
                Category = await _context.Categorys.ToListAsync();
            }
            Templates = _context.PTemplate.ToList();
            //check if default Language Template is used
            string Turl = Templates.Where(e => e.PageTemplateId == Pages.PageTempleteId).FirstOrDefault().TempleteURL;
            string TurlWithFSlash = Turl + "/";
            string path = this.Url.Action().ToString();// Context.HttpContext.Request.Path.Value;
            if (path != Turl && path != TurlWithFSlash)
            {
                
                return Redirect(Turl+"?id=" + id);
            }

            if (Pages == null)
            {
                return NotFound();
            }
            return Page();
        }

        // GET: Orders Total
        public JsonResult OnGetUserEmail()
        {
            var v = "";
            if (SignInManager.IsSignedIn(User))
            {
                v = User.Identity.Name;
            }
            else
            {
                v = "";
            }
            return new JsonResult(v);
        }
    }
}