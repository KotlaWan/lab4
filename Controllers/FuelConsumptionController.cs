using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace laba2.Controllers
{
    public class FuelConsumptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}