using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CoreSite1.Pages.Admin.Service
{
    public class ConfigModel : PageModel
    {
        private readonly long _fileSizeLimit;
        private readonly string _targetFilePath;

        // requires using Microsoft.Extensions.Configuration;
        private readonly IConfiguration Configuration;

        public ConfigModel(IConfiguration configuration)
        {
            Configuration = configuration;
            _fileSizeLimit = configuration.GetValue<long>("FileSizeLimit");

            
            // To save physical files to a path provided by configuration:
            _targetFilePath = configuration.GetValue<string>("StoredFilesPath"); ///dynamic

        }

        [BindProperty]
        public string PageSize { get; set; }

        [BindProperty]
        public string TimerServiceFlag { get; set; }

        [BindProperty]
        public string FileSizeLimit { get; set; }

        public void OnGet()
        {
            PageSize = Configuration["PageSize"];
            TimerServiceFlag = Configuration["TimerServiceFlag"];
            FileSizeLimit = Configuration["FileSizeLimit"];
            //var title = Configuration["Position:Title"];
            //var name = Configuration["Position:Name"];
            var defaultLogLevel = Configuration["Logging:LogLevel:Default"];


            //return Content($"PageSize: {PageSize} \n" +
            //               $"TimerServiceFlag: {TimerServiceFlag} \n" +
            //               $"FileSizeLimit: {FileSizeLimit} \n" +
            //               $"Default Log Level: {defaultLogLevel}");
        }
    
    }
}
