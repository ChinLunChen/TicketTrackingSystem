using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketTrackingSystem.Models
{
    public class BugListViewModel
    {
        public string ID { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }//bug,Feature Request,Test Case
        public string Priority { get; set; }//bug 1.2.3.4.5,Feature Request 0,Test Case 0
        public string Status { get; set; }//Processing,Resolved
        public string CreateUser { get; set; }//QA
        public string CreateDate { get; set; }
        public string EndUser { get; set; }//RD
        public string EndDate { get; set; }
    }
}
