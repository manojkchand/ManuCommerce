using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSite1.Models
{
    public class SizeGuide
    {
        [ScaffoldColumn(false)]
        public int id { get; set; }

        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }

        [ScaffoldColumn(false)]
        public string AddedDate { get; set; }

        public string SizeType { get; set; }
        public string Name { get; set; }

    }
}
