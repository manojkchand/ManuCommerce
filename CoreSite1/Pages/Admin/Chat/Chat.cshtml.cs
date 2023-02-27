using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using WebChatCore.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace WebChatCore.Pages
{
    [Authorize]
    public class ChatModel : PageModel
    {
        private readonly UserManager<ExtendedUser> userManager;
        private readonly IHostingEnvironment _HostEnvironment;

        public ChatModel(UserManager<ExtendedUser> userManager, IHostingEnvironment HostEnvironment)
        {
            this.userManager = userManager;
            //hosting environement path
            _HostEnvironment = HostEnvironment;
        }

        [BindProperty]
        public IEnumerable<ExtendedUser> userlist { get; set; }

        [BindProperty]
        public string userId { get; set; }
        [BindProperty]
        public string userName { get; set; }
        [BindProperty]
        public string userEmail { get; set; }

        public async Task OnGetAsync()
        {
            userlist = userManager.Users;

            Task<ExtendedUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
            var user = await GetCurrentUserAsync();

            userId = user?.Id;
            userEmail = user?.Email;
            userName = user?.Name;
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