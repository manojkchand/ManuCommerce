using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Core.Pages
{
    [Authorize]
    public class BroadcastModel : PageModel
    {
        private readonly UserManager<ExtendedUser> userManager;
        private readonly IHostingEnvironment _HostEnvironment;

        public BroadcastModel(UserManager<ExtendedUser> userManager, IHostingEnvironment HostEnvironment)
        {
            this.userManager = userManager;
            //hosting environement path
            _HostEnvironment = HostEnvironment;
        }

        [BindProperty]
        public IEnumerable<ExtendedUser> userlist { get; set; }

        public void OnGet()
        {
            userlist = userManager.Users;
        }

        //function use to get the user phot from DB. If not found in DB than show a default photo.
        public FileContentResult UserPhotos(byte[] photo)
        {
            if (photo != null)
            {
                return new FileContentResult(photo, "image/jpeg");
            }
            else
            {
                string webRootPath = _HostEnvironment.WebRootPath;

                string fileName = Path.Combine(webRootPath, "Images/DefaultImageless.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }
    }
}