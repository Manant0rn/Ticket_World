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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IConfiguration Configuration { get; }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var client = new HttpClient(clientHandler);
                client.BaseAddress = new Uri(Configuration["WebAPI:URL"]);
                var response = client.GetStringAsync("/DataTicket").Result;
                List<DataTicketModel> GetData = JsonConvert.DeserializeObject<List<DataTicketModel>>(response);
                if(GetData == null)
                {
                    return NotFound();
                }
                else
                {
                    GetData = GetData.OrderBy(e => e.Status).ThenByDescending(e => e.LastUpdateTimestamp).ToList();
                    return View(GetData);
                }
            }
            catch
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
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

        public IActionResult UpdateData(int editid, string editinformation, string editstatus)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri(Configuration["WebAPI:URL"]);
                    EditDataTicketModel UpdateDataTicket = new()
                    {
                        Information = editinformation,
                        Status = editstatus,
                        LastUpdateTimestamp = DateTime.Now
                    };

                    var response = client.PutAsJsonAsync("/DataTicket/" + editid, UpdateDataTicket).Result;

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
