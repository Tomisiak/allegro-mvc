using AllegroMVC.DAL;
using AllegroMVC.Models;
using AllegroMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllegroMVC.Controllers
{
    public class HomeController : Controller
    {
        private StoreContext db = new StoreContext();

        public ActionResult Index()
        {
            var flowerTypes = db.FlowerTypes.ToList();

            var newArrivals = db.Flowers.Where(f => !f.IsHidden) // 3 najnowsze
                .OrderByDescending(f => f.DateAdded)
                .Take(3)
                .ToList();

            var bestsellers = db.Flowers.Where(f => !f.IsHidden && f.IsBestseller) // 3 losowe
                .OrderBy(g => Guid.NewGuid())
                .Take(3)
                .ToList();

            var vm = new HomeViewModel
            {
                Bestsellers = bestsellers,
                FlowerTypes = flowerTypes,
                NewArrivals = newArrivals
            };

            return View(vm);
        }

        public ActionResult StaticContent(string viewname)
        {
            return View(viewname);
        }
    }
}