using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Shopeee.Data;
using Shopeee.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopeeeContext _context;


        public HomeController(ILogger<HomeController> logger, ShopeeeContext context)
        {
            _logger = logger;
            _context = context;
        }
        public class ViewModel
        {
            public List<Brand> ourBrands { get; set; }

        }
        public IActionResult Index()
        {
            ViewModel Viewbrands = new ViewModel();
            //Viewbrands.ourBrands = await _context.Brand.Include();
            var rand = new Random();
            var Brands = _context.Brand.ToList();
            var randBrandIndex = rand.Next(Brands.Count());
            var BrandsToView = new List<Brand>();
            while (BrandsToView.Count() < 4)
                if (!BrandsToView.Contains(Brands[randBrandIndex]))
                {
                    BrandsToView.Add(Brands[randBrandIndex]);
                    randBrandIndex = rand.Next(Brands.Count());
                }
                else
                    randBrandIndex = rand.Next(Brands.Count());

            Viewbrands.ourBrands = BrandsToView;
            if (Viewbrands.ourBrands == null)
            {
                return NotFound();
            }
            return View(Viewbrands);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
