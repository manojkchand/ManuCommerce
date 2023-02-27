using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSite1.Models
{
    public class ToDoItem
    {
        public long Id { get; set; }
        //public string Name { get; set; }
        public string Note { get; set; }
        public bool Done { get; set; }
        public bool IsCalenderEvent { get; set; }
        public DateTime CalenderEventDate { get; set; }
        public String AddedBy { get; set; }
        public DateTime AddedDate { get; set; }

    }

 

    public class Notificaton
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public bool IsNotified { get; set; }
        public String NotificationFor { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
