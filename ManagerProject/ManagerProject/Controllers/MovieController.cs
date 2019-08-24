using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagerProject.Models;

namespace ManagerProject.Controllers
{
    [Route("admin")]
    public class MovieController : Controller
    {
        ManageNetEntities1 db = new ManageNetEntities1();             
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
        
        public ActionResult Delete(int? id)
        {
            try
            {
                Cinema member = db.Cinemas.Find(id);
                member.isDelete = true;
                db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {

                Console.Write(e);
            }
         
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cinema member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }
    }
}