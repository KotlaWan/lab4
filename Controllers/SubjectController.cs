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
    public class SubjectController : Controller
    {
        private ApplicationContext _db;
        private IMemoryCache _cache;
        private string _sessionKey = "formSession";
        private DbService _service;

        public SubjectController(ApplicationContext context, IMemoryCache memoryCache, DbService service)
        {
            _db = context;
            _cache = memoryCache;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Subject(int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 10;

            ViewData["Message"] = "Subjects";

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["TeacherSort"] = sortOrder == SortState.TeacherAsc ? SortState.TeacherDesc : SortState.TeacherAsc;
            ViewData["DescriptionSort"] = sortOrder == SortState.DescriptionAsc ? SortState.DescriptionDesc : SortState.DescriptionAsc;

            var Subjects = _service.GetSubjects(sortOrder);

            var count = await Subjects.CountAsync();
            var items = await Subjects.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            SubjectViewModel viewModel = new SubjectViewModel
            {
                PageViewModel = pageViewModel,
                subjects = items
            };

            return View(viewModel);
        }

        public IActionResult AddSubject()
        {
            ViewData["Message"] = "Add Subject";
            Subject Subject = new Subject();

            if (HttpContext.Session.Keys.Contains(_sessionKey))
            {
                Subject = JsonConvert.DeserializeObject<Subject>(HttpContext.Session.GetString(_sessionKey));
            }

            //ViewBag.PriceFuels = priceFuels;

            return View(Subject);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(Subject subject)
        {
            _db.Subjects.Add(subject);
            _db.SaveChanges();
            //CookieOptions option = new CookieOptions();
            //option.Expires = DateTime.Now.AddSeconds(2 * 10 + 240);
            HttpContext.Session.SetString(_sessionKey, JsonConvert.SerializeObject(subject));
            //_service.AddPriceFuels(priceFuels);
            //_cache.Remove("PriceFuels");

            return View("Subject", await _db.Subjects.ToListAsync());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}