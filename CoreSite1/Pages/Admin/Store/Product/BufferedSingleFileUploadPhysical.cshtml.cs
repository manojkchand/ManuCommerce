using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using SampleApp.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace SampleApp.Pages
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class BufferedSingleFileUploadPhysicalModel : PageModel
    {
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".jpg", ".gif", ".png", ".jpeg" };
        private readonly string _targetFilePath;
        private readonly IHostingEnvironment _HostEnvironment;


        public BufferedSingleFileUploadPhysicalModel(IConfiguration config, IHostingEnvironment HostEnvironment)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");

            // To save physical files to a path provided by configuration:
            _targetFilePath = config.GetValue<string>("StoredFilesPath"); ///dynamic

            // To save physical files to the temporary files folder, use:
            //_targetFilePath = Path.GetTempPath();

            //hosting environement path
            _HostEnvironment = HostEnvironment;
            path = "";
        }

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }

        public string Result { get; private set; }

        [BindProperty]
        public string path { get; set; }

        public void OnGet()
        {
            //to locate ~/wwwroot/CSS
            string webRootPath = _HostEnvironment.WebRootPath;
            string contentRootPath = _HostEnvironment.ContentRootPath;


            path = Path.Combine(webRootPath, "images/FASHION_DATA");
            //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
            //return Page();
           
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            //DeleteFile();
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";

                return Page();
            }
            

            var formFileContent = 
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    FileUpload.FormFile, ModelState, _permittedExtensions, 
                    _fileSizeLimit);

            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";

                return Page();
            }

            // For the file name of the uploaded file stored
            // server-side, use Path.GetRandomFileName to generate a safe
            // random file name.

            //var trustedFileNameForFileStorage = Path.GetRandomFileName();
            //var filePath = Path.Combine(
            //    _targetFilePath, trustedFileNameForFileStorage);
            
            var trustedFileNameForFileStorage = FileUpload.FormFile.FileName;
            var filePath = Path.Combine(
                path, trustedFileNameForFileStorage);


            // **WARNING!**
            // In the following example, the file is saved without
            // scanning the file's contents. In most production
            // scenarios, an anti-virus/anti-malware scanner API
            // is used on the file before making the file available
            // for download or for use by other systems. 
            // For more information, see the topic that accompanies 
            // this sample.

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await fileStream.WriteAsync(formFileContent);

                // To work directly with a FormFile, use the following
                // instead:
                //await FileUpload.FormFile.CopyToAsync(fileStream);
            }

            return RedirectToPage("./Create");
        }


       

              /// <summary>
        /// A method to populate a TreeView with directories, subdirectories, etc
        /// </summary>
        /// <param name="dir">The path of the directory</param>
        /// <param name="node">The "master" node, to populate</param>
        public void PopulateTree(string dir, List<CoreSite1.Models.DTO.PageCategory> node)
        {
            //if (node.children == null)
            //{
            //    node.children = new List<CoreSite1.Models.DTO.PageCategory>();
            //} 
            //// get the information of the directory
            DirectoryInfo directory = new DirectoryInfo(dir);
            // loop through each subdirectory
            foreach (DirectoryInfo d in directory.GetDirectories())
            {
                // create a new node
                CoreSite1.Models.DTO.PageCategory t = new CoreSite1.Models.DTO.PageCategory();
                t.id = d.FullName;
                t.parent = dir;
                t.text = d.Name.ToString();
                t.icon = "jstree-folder";
                // populate the new node recursively
                PopulateTree(d.FullName, node);                              
                node.Add(t); // add the node to the "master" node
            }
            // lastly, loop through each file in the directory, and add these as nodes
            foreach (FileInfo f in directory.GetFiles())
            {
                // create a new node
                CoreSite1.Models.DTO.PageCategory t = new CoreSite1.Models.DTO.PageCategory();
                t.id = f.FullName;
                t.parent = dir;
                t.text = f.Name.ToString();
                t.icon = "jstree-file";
                // add it to the "master"
                node.Add(t);
            }
        }      
             
        public JsonResult OnGetTreeData(string pathpart)
        {
            if(pathpart == null)
            {
                pathpart = "images/FASHION_DATA";
            }
            string webRootPath = _HostEnvironment.WebRootPath;
            string contentRootPath = _HostEnvironment.ContentRootPath;
            path = Path.Combine(webRootPath, pathpart);

            List<CoreSite1.Models.DTO.PageCategory> records = new List<CoreSite1.Models.DTO.PageCategory>();
            //if (AlreadyPopulated == false)
            //{
            CoreSite1.Models.DTO.PageCategory rootNode = new CoreSite1.Models.DTO.PageCategory();
                rootNode.id = path;
                rootNode.text = "ROOT";
                rootNode.parent = "#";
            rootNode.icon = "jstree-folder";
                //a_attr = new CoreSite1.Models.DTO.a_attr(l.CategoryId.ToString(), "/MyStore/details");
                records.Add(rootNode);
                PopulateTree(path, records);
                //AlreadyPopulated = true;

                return new JsonResult(records);
            //}
            //else
            //{
            //    return null;
            //}
        }

        public JsonResult OnGetCreateFolder(string path, string newname)
        {
            Directory.CreateDirectory(path + "\\" + newname);
            //AlreadyPopulated = false;
            return new JsonResult("Created");
        }

        public JsonResult OnGetRenameFolder(string path, string newname)
        {
            //Directory.CreateDirectory(path + "\\" + newname);
            
            Directory.Move(path, Directory.GetParent(path).FullName + "\\" + newname);
          
            return new JsonResult("Renamed");
        }
        //
     

        public JsonResult OnGetDeleteFolder(string path)
        {
            //DeleteFiles(path);
            Directory.Delete(path);
            
            //AlreadyPopulated = false;
            return new JsonResult("Deleted Folder");
        }

        //public JsonResult DeleteFile()
        //{
        //    string path = "C:\\Users\\L\\source\\repos\\CoreSite1\\CoreSite1\\wwwroot\\images\\FASHION_DATA\\71.jpg";
        //    var fileInfo = new System.IO.FileInfo(path);
        //    fileInfo.Delete();
        //    return new JsonResult("Deleted File");
        //}
        public JsonResult OnGetDeleteFile(string path)
        {
            var fileInfo = new System.IO.FileInfo(path);
            fileInfo.Delete();
            return new JsonResult("Deleted File");
        }
        public JsonResult DeleteFiles(string path)
        {
            
            foreach (var file in Directory.GetFiles(path))
            {
                var fileInfo = new System.IO.FileInfo(file);
                fileInfo.Delete();
            }
            return new JsonResult("Deleted Files");
        }
        /// <summary>
        /// /////////////////Deleting file
        /// </summary>

        //private void DeleteFiles()
        //{
        //    //this Directory path has to come from input box
        //    IFileProvider physicalFileProvider = new PhysicalFileProvider(@"D:\DeleteMyContentsPlease\");

        //    if (physicalFileProvider is PhysicalFileProvider)
        //    {
        //        var directory = physicalFileProvider.GetDirectoryContents(string.Empty);
        //        foreach (var file in directory)
        //        {
        //            if (!file.IsDirectory && file.Name == "abc.jpg") //and file is == filename
        //            {
        //                var fileInfo = new System.IO.FileInfo(file.PhysicalPath);
        //                fileInfo.Delete();

        //            }
        //        }
        //    }
        //}
    }

    public class BufferedSingleFileUploadPhysical
    {
        [Required]
        [Display(Name="File")]
        public IFormFile FormFile { get; set; }

        [Display(Name="Note")]
        [StringLength(50, MinimumLength = 0)]
        public string Note { get; set; }
    }

}
