using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ManagerProject.Models;

namespace ManagerProject.Controllers
{ 
    public class HomeController : Controller
    {
        ManageNetEntities1 db = new ManageNetEntities1();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";            
            return View(db.Cinemas);
        }
        private string Mahoa(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputbytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputbytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public ActionResult Buy(int id)
        {
            if (Session["id_member"] == null || Session["id_member"] ==default)
            {
                return RedirectToAction("Index");
            }
            Cinema cinema = db.Cinemas.Find(id);
            ViewBag.Title = "Đặt Vé";
            return View(cinema);
        }
        [HttpPost]
        public ActionResult Buy(BuyMovie buyMovie)
        {
            try
            {
                history _history = new history();
                _history.amount = buyMovie.amount;
                //history.id_member = int.Parse(Session["id_member"].ToString());
                _history.id_member = 1;
                _history.id_movie = buyMovie.IDMovie;
                _history.prices = buyMovie.Ticket * buyMovie.amount;
                _history.date_buy = DateTime.Now;                 
                db.histories.Add(_history);
                db.SaveChanges();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }

        // POST: Index/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID,Username,Password,ConfirmPassword,Email,Money")]Menber_v member)
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
                else if (member.Password != member.ConfirmPassword)
                {
                    ViewBag.DuplicateMessage = "Mật khẩu không khớp";
                }
                else
                {
                    Member member1 = new Member();
                    member1.Email = member.Email;
                    member1.Username = member.Username;
                    member1.Password = Mahoa(member.Password);
                    member1.Money = 10000;
                    db.Members.Add(member1);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(member);
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Login(Member member)
        {
            member.Password = Mahoa(member.Password);
            if ((db.Members.Count(e => e.Username == member.Username) > 0) && (db.Members.Count(e => e.Password == member.Password) > 0))
            {
                Session["id_member"] = db.Members.Where(e => e.Username == member.Username).FirstOrDefault().ID;
                Session["name_member"] = db.Members.Where(e => e.Username == member.Username).FirstOrDefault().Username;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.AccountMessage = "Tài khoản hoặc mật khẩu sai";

            }
            return View();
        }
        public ActionResult History()
        {
            if (Session["id_member"] == null)
            {
                return RedirectToAction("Index");
            }
            int id_member = 0;
            int.TryParse(Session["id_member"].ToString(),out id_member);
            return View(db.histories.Where(e=>e.id_member ==id_member));
        }
    }
}
