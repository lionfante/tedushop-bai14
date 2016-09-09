using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeduShop.Web.Controllers
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
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]   //Không được gọi trực tiếp, chỉ được nhúng
        public ActionResult Footer()
        {
            return PartialView("/Views/Shared/_PartialFooter.cshtml");
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView("/Views/Shared/_PartialHeader.cshtml");
        }

        [ChildActionOnly]
        public ActionResult Category()
        {
            return PartialView("/Views/Shared/_PartialCategory.cshtml");
        }
    }
}