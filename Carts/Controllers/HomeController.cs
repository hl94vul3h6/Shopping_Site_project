using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
namespace Carts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                var result = (from s in db.products select s).ToList();
                return View(result);
            }
        }

        public ActionResult Index2()
        {
            return Content(
                "<html><body><h1>這是一段訊息</h1></body></html>"
                );
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
        public ActionResult Details(int id)
        {
            using (Models.CartsEntities db = new Models.CartsEntities())
            {
                var result = (from s in db.products
                              where s.Id == id
                              select s).FirstOrDefault();

                if (result == default(Models.product))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(result);
                }
            }
        }
        
    }
}