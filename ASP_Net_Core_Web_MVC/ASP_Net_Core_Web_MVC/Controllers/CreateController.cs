using ASP_Net_Core_Web_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ASP_Net_Core_Web_MVC.Controllers
{
    public class CreateController : Controller
    {
        private readonly ILogger<CreateController> _logger;
        public IConfiguration Configuration { get; }

        public CreateController(ILogger<CreateController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddData(string title, string description, string contact, string information)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri(Configuration["WebAPI:URL"]);
                    DataTicketModel AddDataTicket = new()
                    {
                        Title = title,
                        Description = description,
                        Contact = contact,
                        Information = information,
                        Status = "Pending",
                        CreateTimestamp = DateTime.Now,
                        LastUpdateTimestamp = DateTime.Now
                    };

                    var response = client.PostAsJsonAsync("/DataTicket", AddDataTicket).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index","Home");
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
