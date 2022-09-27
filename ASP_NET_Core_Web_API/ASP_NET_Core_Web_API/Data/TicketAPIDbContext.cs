using ASP_Net_Core_Web_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_Core_Web_API.Data
{
    public class TicketAPIDbContext : DbContext
    {
        public TicketAPIDbContext(DbContextOptions<TicketAPIDbContext> options)
            : base(options)
        {
        }

        public DbSet<DataTicketModel> DataTicket { get; set; }
    }
}
