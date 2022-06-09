using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TicketTrackingSystem.Models;

namespace TicketTrackingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                //0.controller 1.action 2.user
                string sUser = Request.RouteValues.Values.ElementAt(2).ToString();
                Response.Cookies.Append("User", sUser);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Permission()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string sUser)
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
