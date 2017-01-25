using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AllegroMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "logowanie",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Register",
                url: "rejestracja",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "Cart",
                url: "koszyk",
                defaults: new { controller = "Cart", action = "Index" }
            );

            routes.MapRoute(
                name: "Checkout",
                url: "koszyk/dane-zamowienia",
                defaults: new { controller = "Cart", action = "Checkout" }
            );

            routes.MapRoute(
                name: "OrderConfirmation",
                url: "koszyk/potwierdzenie-zamowienia",
                defaults: new { controller = "Cart", action = "OrderConfirmation" }
            );

            routes.MapRoute(
                name: "Profile",
                url: "profil",
                defaults: new { controller = "Manage", action = "Index" }
            );

            routes.MapRoute(
                name: "Orders",
                url: "profil/zamowienia",
                defaults: new { controller = "Manage", action = "OrdersList" }
            );

            routes.MapRoute(
                name: "ProductDetails",
                url: "kwiat-{id}",
                defaults: new { controller = "Store", action = "Details" }
            );

            routes.MapRoute(
                name: "StaticPages",
                url: "{viewname}",
                defaults: new { controller = "Home", action = "StaticContent" }
            );

            routes.MapRoute(
                name: "ProductList",
                url: "gatunki/{flowerTypeName}",
                defaults: new { controller = "Store", action = "List" },
                constraints: new { flowerTypeName = @"[\w& ]+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
