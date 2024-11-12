using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Osbar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            if (Session["Usuario"] != null)
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
            else
            {
                return RedirectToAction("Login","Inicio");
            }
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            Session.Abandon();
            return RedirectToAction("Login","Inicio");
        }
    }
}