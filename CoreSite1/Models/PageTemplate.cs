using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSite1.Models
{
    public class PageTemplate
    {
        [ScaffoldColumn(false)]
        public int PageTemplateId { get; set; }

        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime AddedDate { get; set; }

        
        public string Name { get; set; }

        //[StringLength(350)]
        public string TempleteURL { get; set; }

        public string Header { get; set; }

        public string Footer { get; set; }

        public string SideBar { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
