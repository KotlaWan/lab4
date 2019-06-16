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
using laba2.Filters;

namespace laba2.Controllers
{
    public class PriceClassController : Controller
    {
        private ApplicationContext _db;
        private IMemoryCache _cache;
        private string _cookieKey = "formCookies";
        private DbService _service;

        public PriceClassController(ApplicationContext context, IMemoryCache memoryCache, DbService service)
        {
            _db = context;
            _cache = memoryCache;
            _service = service;
        }

        [LoggingFilter]
        public IActionResult Index()
        {
            return View();
        }

        [SaveStoreFilter("sort")]
        [LoggingFilter]
        public async Task<IActionResult> PriceList(string classType = null, int page = 1, SortState sortOrder = SortState.ClassLeadAsc)
        {
            //if (sortOrder != SortState.No) HttpContext.Session.SetString("sort", JsonConvert.SerializeObject(sortOrder));
            //else
            //{
            //    if (HttpContext.Session.Keys.Contains("sort"))
            //        sortOrder = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("sort"));
            //    else sortOrder = SortState.TypeAsc;
            //}
            Dictionary<string, string> dict;
            if (HttpContext.Session.Keys.Contains("sort"))
            {
                //dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(HttpContext.Session.GetString("sort"));
                dict = HttpContext.Session.Get<Dictionary<string, string>>("sort");
                if (dict.ContainsKey("type")) classType = dict["type"];
                if (dict.ContainsKey("sortOrder")) sortOrder = Enum.Parse<SortState>(dict["sortOrder"]);
            }
            //sortOrder = SortState.TypeAsc;
            int pageSize = 10;

            ViewData["Message"] = "Price-list";

            ViewData["ClassTypesort"] = sortOrder == SortState.ClassTypeAsc ? SortState.ClassTypeDesc : SortState.ClassTypeAsc;
            ViewData["ClassLeadSort"] = sortOrder == SortState.ClassLeadAsc ? SortState.ClassLeadDesc : SortState.ClassLeadAsc;
            ViewData["PriceSort"] = sortOrder == SortState.CountAsc ? SortState.CountDesc : SortState.CountAsc;
            ViewData["DateSort"] = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;


            //List<PriceClass> cacheList = _cache.Get<List<PriceClass>>("PriceClass");

            IQueryable<PriceClass> cacheList = _service.GetPriceFuels(classType, sortOrder);

            var count = await cacheList.CountAsync();
            var items = await cacheList.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            PriceClassViewModel viewModel = new PriceClassViewModel
            {
                PageViewModel = pageViewModel,
                PriceClass = items,
                Type = classType
            };

            return View(viewModel);
        }

        [LoggingFilter]
        public IActionResult AddFuel()
        {
            ViewData["Message"] = "Add fuel date";
            List<string> TypeClass = new List<string> { "1", "2", "3", "4" };
            ViewBag.TypeClass = new SelectList(TypeClass);
            PriceClass priceClass = new PriceClass("1", "11", 1, DateTime.Now);

            if (Request.Cookies.ContainsKey(_cookieKey))
            {
                priceClass = JsonConvert.DeserializeObject<PriceClass>(Request.Cookies[_cookieKey]);
            }

            //ViewBag.PriceClass = priceClass;

            return View(priceClass);
        }

        [LoggingFilter]
        [HttpPost]
        public IActionResult AddFuel(PriceClass priceClass)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddSeconds(2 * 10 + 240);
            Response.Cookies.Append(_cookieKey, JsonConvert.SerializeObject(priceClass), option);
            _service.AddPriceFuels(priceClass);
            _cache.Remove("PriceClass");

            return View("PriceList", _service.GetPriceFuels());
        }

        [LoggingFilter]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}