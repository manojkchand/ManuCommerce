using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin.Store.Users
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class DetailModel : PageModel
    {
        private readonly UserManager<ExtendedUser> userManager;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly CoreSite1.Data.ApplicationDbContext _context;
        public IList<CoreSite1.Models.Address> Addresses { get; set; }

        public DetailModel(CoreSite1.Data.ApplicationDbContext context,UserManager<ExtendedUser> userManager, IHostingEnvironment HostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.userManager = userManager;
            //hosting environement path
            _HostEnvironment = HostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public ExtendedUser user { get; set; }
        //IEnumerable<ExtendedUser>
        [BindProperty]
        public string userId { get; set; }
        [BindProperty]
        public string userName { get; set; }
        [BindProperty]
        public string userEmail { get; set; }

        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string id)
        {
            //string pk = User.Identity.Name.ToString();
            //var tempadd = _context.Addresses.ToList();
            //Below GETS THE ADDRESS OF LOGED IN USER
            //Addresses = _context.Addresses.Where(e => e.AddedBy.Trim() == User.Identity.Name).ToList();
            //return View(users);///////////////////////////////////////
            
            //Task<ExtendedUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
            user = userManager.Users.Where(e => e.Id == id).FirstOrDefault();//GetCurrentUserAsync();userManager.Users

            Addresses = _context.Addresses.Where(e => e.AddedBy.Trim() == user.Email).ToList();

            userId = user?.Id;
            userEmail = user?.Email;
            userName = user?.Name;

            ////uses class claimprincipalextension//delete the class if not using below code
            ///and remove startup.cs- services.TryAddSingleton IHttpContextAccessor, HttpContextAccessor>();///////////
            //userName = _httpContextAccessor.HttpContext.User.GetLoggedInUserName();
            //if (userName == null) { userName = "Null"; }
            //userEmail = _httpContextAccessor.HttpContext.User.GetLoggedInUserEmail();
            //if (userEmail == null) { userEmail = "Null"; }
            //userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<string>(); // Specify the type of your UserId;
            //if (userId == null) { userId = "Null"; }



        }

        public FileContentResult UserPhotos(byte[] photo)
        {
            if (photo != null)
            {
                return new FileContentResult(photo, "image/jpeg");
            }
            else
            {
                string webRootPath = _HostEnvironment.WebRootPath;
                //string contentRootPath = _HostEnvironment.ContentRootPath;


                string fileName = Path.Combine(webRootPath, "Images/DefaultImageless.png");
                //string fileName = Url.Content("~/Images/DefaultImageless.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }

            //if (User.Identity.IsAuthenticated)
            //{
            //    String userId = User.Identity.GetUserId();

            //    if (userId == null)
            //    {
            //        string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

            //        byte[] imageData = null;
            //        FileInfo fileInfo = new FileInfo(fileName);
            //        long imageFileLength = fileInfo.Length;
            //        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //        BinaryReader br = new BinaryReader(fs);
            //        imageData = br.ReadBytes((int)imageFileLength);

            //        return File(imageData, "image/png");

            //    }
            //    // to get the user details to load user Image
            //    var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            //    var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

            //    return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            //}
            //else
            //{
            //    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

            //    byte[] imageData = null;
            //    FileInfo fileInfo = new FileInfo(fileName);
            //    long imageFileLength = fileInfo.Length;
            //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            //    BinaryReader br = new BinaryReader(fs);
            //    imageData = br.ReadBytes((int)imageFileLength);
            //    return File(imageData, "image/png");

            //}
        }
    }
}
