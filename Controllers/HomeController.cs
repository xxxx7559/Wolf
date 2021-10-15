using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SignalRSample.Models;
using Newtonsoft.Json;
using SignalRSample.DB;

namespace SignalRSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbServices _dbServices;

        public HomeController(ILogger<HomeController> logger, IDbServices dbServices)
        {
            _logger = logger;
            _dbServices = dbServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Host()
        {
            var wJsons = _dbServices.GetWolfTypes();
            //var sr = new StreamReader("./w_type.json");
            //var st = sr.ReadToEnd().Replace("\r\n", "");
            //var w_jsons = JsonConvert.DeserializeObject<List<WolfType>>(st);
            //sr.Close();

            ViewBag.WolfType = new SelectList(wJsons, "Id", "Type");
            return View();
        }

        public IActionResult Player()
        {
            return View();
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