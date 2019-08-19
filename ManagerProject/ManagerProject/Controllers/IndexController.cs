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
    public class IndexController : Controller
    {
        private MemberEntities db = new MemberEntities();
        private AdminEntities dbadmin = new AdminEntities();

        // GET: Index
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }
        public string Mahoa(string input )
        {
            MD5 md5 = MD5.Create();
            byte[] inputbytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputbytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0;i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
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

        // GET: Index/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Index/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Password,Email,Money")]Member member)
        {            
            if (ModelState.IsValid)
            {
                if (db.Members.Count(e => e.Username == member.Username) > 0)
                {
                    ViewBag.DuplicateMessage = "Tài khoản đã được sử dụng";
                }
                else if (db.Members.Count(e => e.Email == member.Email) > 0)
                {
                    ViewBag.EmailMessage = "Email đã được sử dụng";
                }
                else
                {
                    db.Members.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Member member)
        {
            if ((db.Members.Count(e => e.Username == member.Username) > 0) && (db.Members.Count(e => e.Password == member.Password) > 0))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.AccountMessage = "Tài khoản hoặc mật khẩu sai";

            }
            return View();
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
                if (dbadmin.Administrators.Count(e => e.Username != admin.Username) >0)
                {
                    ViewBag.AccountMessage = "Tài khoản không tồn tại.";
                }
                else if ((dbadmin.Administrators.Count(e => e.Username == admin.Username) >0) && (dbadmin.Administrators.Count(e => e.Password != admin.Password) >0))
                {
                    ViewBag.AccountMessage = "Mật khẩu sai, vui lòng nhập lại.";
                }
                else if ((dbadmin.Administrators.Count(e => e.Username == admin.Username) > 0) && (dbadmin.Administrators.Count(e => e.Password == admin.Password) > 0))
                {
                    RedirectToAction("Index");
                }
            }
            return View(admin);
        }

    }
}
