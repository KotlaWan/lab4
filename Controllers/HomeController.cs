using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using laba2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using laba2.Services;

namespace laba2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _db;
        private IMemoryCache _cache;
        private string _cookieKey = "formCookies";
        private string _sessionKey = "formSession";
        private DbService _service;

        public HomeController(ApplicationContext context, IMemoryCache memoryCache, DbService service)
        {
            _db = context;
            _cache = memoryCache;
            _service = service;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Index()
        {
            return View();
        }

        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
