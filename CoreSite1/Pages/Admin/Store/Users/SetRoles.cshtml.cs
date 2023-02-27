using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoreSite1.Pages.Admin.Store.Users
{
    public class SetRolesModel : PageModel
    {
        private readonly UserManager<ExtendedUser> userManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        public SetRolesModel(UserManager<ExtendedUser> userManager, RoleManager<IdentityRole> RoleManager)
        {
            this.userManager = userManager;
            this.RoleManager = RoleManager;
        }

        [BindProperty]
        public List<IdentityRole> rolelist { get; set; }
        [BindProperty]
        public ExtendedUser user { get; set; }
        [BindProperty]
        public IList<string> userRole { get; set; }
        [BindProperty]
        public string selectedRole { get; set; }
        public async Task OnGetAsync(string id,string rl)
        {
            rolelist = RoleManager.Roles.ToList();

            if(id != null) 
            { 
                user = userManager.Users.Where(e => e.Id == id).FirstOrDefault();
                userRole = (IList<string>)await userManager.GetRolesAsync(user);

                //ExtendedUser user = await UserManager.FindByEmailAsync("manojchand@gmail.com");
                ////var User = new IdentityUser();
                ///
                if(rl != null) 
                {
                    if(userRole.Contains(rl.ToString()))
                    {
                        await userManager.RemoveFromRoleAsync(user, rl); 
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, rl);
                    }
                }
                //get all new rolles new role list. (If added or removed a role.)
                userRole = (IList<string>)await userManager.GetRolesAsync(user);

            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //ExtendedUser user = await UserManager.FindByEmailAsync("manojchand@gmail.com");
            ////var User = new IdentityUser();
            await userManager.AddToRoleAsync(user, selectedRole);


            return Page();
        }

    }
}
