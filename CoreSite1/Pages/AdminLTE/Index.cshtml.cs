using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreSite1.Pages.AdminLTE
{
    [Authorize(Roles = "ThisSiteAdmin")] // [Authorize(Roles = "Admin,SuperAdmin,Manager")]

    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}