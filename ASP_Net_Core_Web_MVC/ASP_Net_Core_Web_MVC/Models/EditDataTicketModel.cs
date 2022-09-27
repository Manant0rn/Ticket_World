using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Net_Core_Web_MVC.Models
{
    public class EditDataTicketModel
    {
        public string Information { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdateTimestamp { get; set; }
    }
}
