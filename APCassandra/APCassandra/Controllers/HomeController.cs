using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APCassandra.Models;
using Cassandra;

namespace APCassandra.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISession _session;

        public HomeController(ILogger<HomeController> logger, ISession session)
        {
            _logger = logger;
            _session = session;
        }

        public IActionResult Index()
        {
            //Execute a query on a connection synchronously
            var rs = _session.Execute("SELECT * FROM auto_by_id");
            //Iterate through the RowSet
            foreach (var row in rs)
            {
                var value = row.GetValue<int>("");
                //do something with the value
            }
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
