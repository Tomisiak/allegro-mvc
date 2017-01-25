using AllegroMVC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace AllegroMVC.Controllers
{
    public class StoreController : Controller
    {
        StoreContext db = new StoreContext(); 

        public ActionResult Details(int id)
        {
            ViewBag.AllegroAuction = TempData["AllegroAuction"];
            var flower = db.Flowers.Find(id);

            return View(flower);
        }

        public ActionResult List(string flowerTypeName, string searchQuery = null)
        {
            var flowerType = db.FlowerTypes.Include("Flowers")
                .Where(ft => ft.Name.ToUpper() == flowerTypeName.ToUpper())
                .Single();
            var flowers = flowerType.Flowers.Where(f => (searchQuery == null || f.FlowerName.ToLower().Contains(searchQuery.ToLower())) && !f.IsHidden);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", flowers);
            }

            return View(flowers);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 80000)]
        public ActionResult FlowerTypesMenu()
        {
            var flowerTypes = db.FlowerTypes.ToList();

            return PartialView("_FlowerTypesMenu", flowerTypes);
        }

        public ActionResult FlowersSuggestions(string term)
        {
            var flowers = db.Flowers.Where(f => !f.IsHidden && f.FlowerName.ToLower().Contains(term.ToLower()))
                .Take(5)
                .Select(a => new { label = a.FlowerName });

            return Json(flowers, JsonRequestBehavior.AllowGet);
        }
    }
}