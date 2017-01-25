using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllegroMVC.App_Start
{
    public class UserRoleActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                filterContext.Controller.ViewBag.UserIsAdmin = true;
            }
            else
            {
                filterContext.Controller.ViewBag.UserIsAdmin = false;
            }
        }
    }

    public class PageTitleActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Title = "Witaj w kwiaciarni Chryzantema!";
        }
    }
}