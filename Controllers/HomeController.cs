using Rotativa;
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

        public ActionResult FundamentosEmpresa()
        {
                ViewBag.Message = "Valores y Principios.";

                return View();
            
        }

       
        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            Session.Abandon();
            return RedirectToAction("Login","Inicio");
        }
    }
}