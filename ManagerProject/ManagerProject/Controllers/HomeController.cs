using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagerProject.Models;

namespace ManagerProject.Controllers
{
    public class HomeController : Controller
    {
        MemberEntities db = new MemberEntities();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";            
            return View(db.Cinemas);
        }
    }
}
