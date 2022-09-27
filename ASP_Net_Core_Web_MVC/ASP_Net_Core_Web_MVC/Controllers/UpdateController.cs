using ASP_Net_Core_Web_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ASP_Net_Core_Web_MVC.Controllers
{
    public class UpdateController : Controller
    {
        private readonly ILogger<UpdateController> _logger;
        public IConfiguration Configuration { get; }

        public UpdateController(ILogger<UpdateController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IActionResult Index(int id)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(Configuration["WebAPI:URL"]);
            var response = client.GetStringAsync("/DataTicket/"+ id).Result;
            DataTicketModel GetData = JsonConvert.DeserializeObject<DataTicketModel>(response);
            if(GetData == null)
            {
                return NotFound();
            }
            else
            {
                return View(GetData);
            }
            
        }

        public IActionResult UpdateData(int id,string title, string description, string contact, string information,string status)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri(Configuration["WebAPI:URL"]);
                    DataTicketModel UpdateDataTicket = new()
                    {
                        Title = title,
                        Description = description,
                        Contact = contact,
                        Information = information,
                        Status = status,
                        LastUpdateTimestamp = DateTime.Now
                    };

                    var response = client.PutAsJsonAsync("/DataTicket/"+id, UpdateDataTicket).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(response.StatusCode.ToString(), response.ReasonPhrase);
                    }

                }
                return View();
            }
            catch
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
