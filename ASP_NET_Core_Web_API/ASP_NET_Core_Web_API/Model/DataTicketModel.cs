using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Net_Core_Web_API.Models
{
    public class DataTicketModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
        public string Information { get; set; }
        public string Status { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public DateTime LastUpdateTimestamp { get; set; }
    }
}
