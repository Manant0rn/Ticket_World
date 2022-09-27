using ASP_Net_Core_Web_API.Models;
using ASP_NET_Core_Web_API.Data;
using ASP_Net_Core_Web_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_Core_Web_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataTicketController : ControllerBase
    {
        private readonly ILogger<DataTicketController> _logger;
        private readonly TicketAPIDbContext dbContext;

        public DataTicketController(ILogger<DataTicketController> logger, TicketAPIDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTicket()
        {
            try
            {
                return Ok(await dbContext.DataTicket.ToListAsync());
            }
            catch(Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetTicketById([FromRoute] int id)
        {
            try
            {
                DataTicketModel GetTicket = await dbContext.DataTicket.FindAsync(id);
                if(GetTicket == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(GetTicket);
                }
            }
            catch(Exception ex)
            {
                return Ok(ex);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(DataTicketModel AddDataTicket)
        {
            try
            {
                await dbContext.DataTicket.AddAsync(AddDataTicket);
                await dbContext.SaveChangesAsync();
                return Ok(AddDataTicket);
            }
            catch(Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateTicketById([FromRoute] int id, EditDataTicketModel UpdateDataTicket)
        {
            try
            {
                DataTicketModel GetTicket = await dbContext.DataTicket.FindAsync(id);
                if(GetTicket != null)
                {
                    GetTicket.Information = UpdateDataTicket.Information;
                    GetTicket.Status = UpdateDataTicket.Status;
                    GetTicket.LastUpdateTimestamp = UpdateDataTicket.LastUpdateTimestamp;

                    await dbContext.SaveChangesAsync();

                    return Ok(GetTicket);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteTicketById([FromRoute] int id)
        {
            try
            {
                DataTicketModel GetTicket = await dbContext.DataTicket.FindAsync(id);
                if (GetTicket == null)
                {
                    return NotFound();
                }
                else
                {
                    dbContext.DataTicket.Remove(GetTicket);
                    await dbContext.SaveChangesAsync();
                    return Ok(GetTicket);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

    }
}
