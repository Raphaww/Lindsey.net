using LindseyTour.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LindseyTour.Controllers
{
   public class HomeController : Controller
   {
      //
      // GET: /Home/
      public ActionResult Index()
      {
         var experiment = Experiments.GetExperimentByUrl(System.Web.HttpContext.Current, "miUrl.com");
         ViewBag.experiment = experiment;
         return View();
      }

      public ActionResult ThankYou()
      {
         return View();
      }
   }
}