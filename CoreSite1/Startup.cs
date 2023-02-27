using CoreSite1.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using CoreSite1.Services;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;



namespace CoreSite1
{
    public class Startup
    {
        // We use a key generated on this server during startup to secure our tokens.
        // This means that if the app restarts, existing tokens become invalid. It also won't work
        // when using multiple servers.
        public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ExtendedUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages();

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            if (Configuration.GetValue<string>("TimerServiceFlag").ToString() == "On")
            {
                 services.AddHostedService<TimedHostedService>();
                // services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.Use(async (context, next) =>
            //{
            //    var url = context.Request.Path.Value;

            //    // Rewrite to index
            //    if (url == "/Index")
            //    {
            //        // rewrite and continue processing
            //        context.Request.Path = "/MyStore";
            //    }

            //    await next();
            //});



            //custom
            var options = new RewriteOptions();
            options.Rules.Add(new CustomRule());
            app.UseRewriter(options);

            //app.UseRewriter(new RewriteOptions()
            // .AddRedirectToHttps());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSignalR(route =>
            {
                route.MapHub<ChatHub>("/chathub");
            });
            //app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

             //CreateUserRoles(services).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ExtendedUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("ThisSiteAdmin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("ThisSiteAdmin"));
            }
            //Adding Customer Role
            var roleCheck2 = await RoleManager.RoleExistsAsync("SiteCustomer");
            if (!roleCheck2)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("SiteCustomer"));
            }
            //Adding Supplier Role
            var roleCheck3 = await RoleManager.RoleExistsAsync("Supplier");
            if (!roleCheck3)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Supplier"));
            }
            //Adding Bidder Role
            var roleCheck4 = await RoleManager.RoleExistsAsync("Bidder");
            if (!roleCheck4)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Bidder"));
            }
            //Adding SiteEditor Role
            var roleCheck5 = await RoleManager.RoleExistsAsync("ThisSiteEditor");
            if (!roleCheck5)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("ThisSiteEditor"));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            //ExtendedUser user = await UserManager.FindByEmailAsync("manojchand@gmail.com");
            ////var User = new IdentityUser();
            //await UserManager.AddToRoleAsync(user, "ThisSiteAdmin");
        }
    }
}
