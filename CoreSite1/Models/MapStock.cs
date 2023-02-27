using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreSite1.Models
{
    public class MapStock
    {
        [ScaffoldColumn(false)]
        public int MapStockID { get; set; }

        [ScaffoldColumn(false)]
        public DateTime AddedDate { get; set; }

        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }



        public int MapImageID { get; set; }
        public int ProductId { get; set; }

        public List<Product> Productlist { get; set; }
        public List<MapImage> MapImagelist { get; set; }
    }
}
