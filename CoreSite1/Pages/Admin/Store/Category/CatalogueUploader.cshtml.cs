using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreSite1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreSite1.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Category
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class CatalogueUploaderModel : PageModel
    {
        public void OnGet()
        {

        }
        public CoreSite1.Models.FileUpload FileUpload;

        public async Task<IActionResult> OnPostAsync()
        {
            // Perform an initial check to catch FileUpload class attribute violations.
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var filePath = "<PATH-AND-FILE-NAME>";
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await FileUpload.UploadPublicSchedule.CopyToAsync(fileStream);
            }
            return RedirectToPage("./Index");
        }
    }
}