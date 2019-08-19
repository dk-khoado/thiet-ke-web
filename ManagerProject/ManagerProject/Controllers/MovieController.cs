using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagerProject.Models;

namespace ManagerProject.Controllers
{    
    public class MovieController : Controller
    {
        MemberEntities db = new MemberEntities();             
        // GET: Movie
        public ActionResult Index()
        {
            return View(db.Cinemas);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                db.Cinemas.Add(cinema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
    }
}