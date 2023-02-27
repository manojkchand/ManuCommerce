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
    public class MapImage
    {
        [ScaffoldColumn(false)]
        public int MapImageID { get; set; }

        [ScaffoldColumn(false)]
        public DateTime AddedDate { get; set; }

        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }


        public string LocationName { get; set; }

        public Mapshape MapShape { get; set; }

        public string coords { get; set; }
        //multiple images can be maped.
        public int ImageID { get; set; }
    }

    public enum Mapshape
    {
        rect,
        circle,
        poly
    }
}
