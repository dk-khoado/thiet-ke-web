using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Xml.Serialization;
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
                admin _admin = new admin();
                string path = HttpContext.Server.MapPath("~/App_Data/admin_config.xml");
                XmlSerializer xml = new XmlSerializer(typeof(admin));
                using (Stream reader = new FileStream(path, FileMode.Open))
                {
                    _admin = (admin)xml.Deserialize(reader);
                }
                if (_admin.username != admin.Username)
                {
                    ViewBag.AccountMessage = "Tài khoản không tồn tại.";
                }
                else if (_admin.password != admin.Password)
                {
                    ViewBag.AccountMessage = "Mật khẩu sai, vui lòng nhập lại.";
                }
                else if (_admin.password == admin.Password && _admin.username == admin.Username)
                {
                    Session["admin"] = _admin.username;
                    return RedirectToAction("Index");
                }
            }
            return View(admin);
        }
        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("LoginAdmin");
        }
    }
}
