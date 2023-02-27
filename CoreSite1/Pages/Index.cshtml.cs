using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoreSite1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<ExtendedUser> SignInManager;
        private readonly UserManager<ExtendedUser> UserManager;

        public IndexModel(ILogger<IndexModel> logger, SignInManager<ExtendedUser> _SignInManager, UserManager<ExtendedUser> _UserManager)
        {
            _logger = logger;
            SignInManager = _SignInManager;
            UserManager = _UserManager;
        }

        public void OnGet()
        {
            if (!SignInManager.IsSignedIn(User))
            {

                Response.Redirect("/MyStore/Index");

            }
            if (!User.IsInRole("ThisSiteAdmin"))
            {
                Response.Redirect("/MyStore/Index");
            }
        }
    }
}
