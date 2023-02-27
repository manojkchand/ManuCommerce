using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SampleApp.Utilities;

namespace CoreSite1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ExtendedUser> _signInManager;
        private readonly UserManager<ExtendedUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ExtendedUser> userManager,
            SignInManager<ExtendedUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full name")]
            public string Name { get; set; }

            [Display(Name = "Photo")]
            public byte[] Photo { get; set; }

            [Display(Name = "Birth Date")]
            [DataType(DataType.Date)]
            public DateTime DOB { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (FileUpload.FormFile != null)
                {
                    // To convert the user uploaded Photo as Byte Array before save to DB
                    
                    if (Request.Form.Files.Count > 0)
                    {

                        IFormFile poImgFile = Request.Form.Files["Photo"];

                        //using (var binary = new BinaryReader(poImgFile.OpenReadStream()))
                        //{
                        //    imageData = binary.ReadBytes((int)poImgFile.Length);
                        //}

                        Image pThmbnai;
                        pThmbnai = new Bitmap(FileUpload.FormFile.OpenReadStream()).GetThumbnailImage(100, 100, null, new IntPtr());

                        //convert to jpeg //size decreased
                        var ms = new MemoryStream();
                        pThmbnai.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                        
                        imageData = ms.ToArray();
                    }
                }

                var user = new ExtendedUser
                { UserName = Input.Email, Email = Input.Email,
                    Name = Input.Name,
                    Photo = imageData,
                    DOB = Input.DOB
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    //can use GetToken() method return below.
                    // add the email claim and value for this user
                   await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, Input.Email));

                 


                    //beow funciton was giving error probabily because of externed user and not IdentityUser.
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = "code" },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

       
    }
}
