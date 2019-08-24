using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ManagerProject.Models;

namespace ManagerProject.Controllers
{
   [Route("admin")]
    public class MemberController : Controller
    {
        private ManageNetEntities1 db = new ManageNetEntities1();

        // GET: Index             
        public ActionResult Index()
        {
            return View(db.Members.ToList());            
        }       
       
        // GET: Index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
                

        // GET: Index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Index/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Index/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Index/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }       
        //GET
        public ActionResult LoginAdmin()
        {
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult LoginAdmin(Administrator admin)
        {
            if (ModelState.IsValid)
            {
                if (db.Administrators.Count(e => e.Username != admin.Username) >0)
                {
                    ViewBag.AccountMessage = "Tài khoản không tồn tại.";
                }
                else if ((db.Administrators.Count(e => e.Username == admin.Username) >0) && (db.Administrators.Count(e => e.Password != admin.Password) >0))
                {
                    ViewBag.AccountMessage = "Mật khẩu sai, vui lòng nhập lại.";
                }
                else if ((db.Administrators.Count(e => e.Username == admin.Username) > 0) && (db.Administrators.Count(e => e.Password == admin.Password) > 0))
                {
                    RedirectToAction("Index");
                }
            }
            return View(admin);
        }

    }
}
