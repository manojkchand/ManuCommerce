using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreSite1.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
// EF dependencies:
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;

namespace CoreSite1
{
    //public class CustomRule
    //{
    //}
    public class CustomRule : Microsoft.AspNetCore.Rewrite.IRule
    {
        //private IConfiguration configuration;
        //public CustomRule(IConfiguration iConfig)
        //{
        //    configuration = iConfig;
        //}
        //private string _connectionString = configuration.GetValue<string>("MySettings:DbConnection"); //
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-CMSCore-53bc9b9d-9d6a-45d4-8429-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true";
        //NOTEITOUT//Configuration.GetValue("PageSize", 10);//suppose to work in core.3.0+
        public void ApplyRule(RewriteContext context)
        {
            IList<CoreSite1.Models.Page> v;
            IList<CoreSite1.Models.PageTemplate> Templates;
            var url = context.HttpContext.Request.Path.Value;

            // In this example
            // "ApplicationDbContext" is my DbContext class
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(_connectionString).Options;
            // With the options generated above, we can then just construct a new DbContext class
            
            //Get Data from Database
            using (var ctx = new ApplicationDbContext(options))
            {//get only two fields url and pageid
                v = (from p in ctx.Pages
                    select new CoreSite1.Models.Page
                    { URL = p.URL,
                      PageId = p.PageId,
                      PageTempleteId=p.PageTempleteId
                    }).ToList();

                Templates = ctx.PTemplate.ToList();
            }
            //Check URL and rewrite
            foreach (var vari in v)
            {
                if (vari.URL != null) { 
                if (url == vari.URL)
                {
                        // rewrite and continue processing
                        //var host = context.HttpContext.Request.Host.ToString();?id=2
                        var t = Templates.Where(e => e.PageTemplateId == vari.PageTempleteId).FirstOrDefault().TempleteURL;//"/Home/Index";
                        //context.HttpContext.Request.QueryString.Add("id", vari.PageId.ToString());

                        context.HttpContext.Request.QueryString = Microsoft.AspNetCore.Http.QueryString.Create("id", vari.PageId.ToString());
                        context.HttpContext.Request.Path = t;
                        break;
                }
                }
            }

            //// Rewrite to index
            //if (url.Contains("/Privacy"))
            //{
            //    // rewrite and continue processing
            //    context.HttpContext.Request.Path = "/Index";
            //}

            
            //var request = context.HttpContext.Request;
            //var host = request.Host;
            //if (host.Host.Contains("localhost",
            // StringComparison.OrdinalIgnoreCase))
            //{
            //    if (host.Port == 80)
            //    {
            //        context.Result = RuleResult.ContinueRules;
            //        return;
            //    }
            //}
            //var response = context.HttpContext.Response;
            //response.StatusCode = (int)HttpStatusCode.BadRequest;
            //context.Result = RuleResult.EndResponse;
        }
    }
}
