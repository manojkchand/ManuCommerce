using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreSite1.Data;
using Microsoft.Extensions.FileProviders;
using SampleApp.Utilities;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly SignInManager<ExtendedUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".jif", ".png" };
        private readonly IHostingEnvironment _HostEnvironment;

        public IndexModel(
            UserManager<ExtendedUser> userManager,
            SignInManager<ExtendedUser> signInManager,
            IEmailSender emailSender,
            IHostingEnvironment HostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _HostEnvironment = HostEnvironment;
        }
        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }


        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full name")]
            public string Name { get; set; }

            [Display(Name = "PhotoFile")]
            public byte[] FileUpload { get; set; }

            [Display(Name = "Birth Date")]
            [DataType(DataType.Date)]
            public DateTime DOB { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;



            Input = new InputModel
            {
                Email = email,
                Name = user.Name,
                FileUpload = user.Photo,
                //Photo = user.Photo,
                DOB = user.DOB,
                PhoneNumber = phoneNumber
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
          
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }



            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

           
            if (FileUpload.FormFile != null)
            {
                // To convert the user uploaded Photo as Thumbnail and Byte Array before save to DB
                
                //make bitmap impage thumbnail
                Image pThmbnai;
                pThmbnai = new Bitmap(FileUpload.FormFile.OpenReadStream()).GetThumbnailImage(100, 100, null, new IntPtr());
                
                //convert to jpeg //size decreased
                var ms = new MemoryStream();
                pThmbnai.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] imageData = null;                
                imageData = ms.ToArray();

                ////direct photo upload
                //imageData =  await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                //FileUpload.FormFile, ModelState, _permittedExtensions,
                //2097152);

                user.Photo = imageData;
            }


            if (Input.DOB != user.DOB)
            {
                user.DOB = Input.DOB;
            }

            await _userManager.UpdateAsync(user);


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        //view user photo
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
        }
    }
}
