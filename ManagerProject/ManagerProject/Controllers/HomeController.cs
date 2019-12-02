using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            if (Session["id_member"] == null || Session["id_member"] == default)
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
                _history.id_member = int.Parse(Session["id_member"].ToString());
                _history.id_movie = buyMovie.IDMovie;
                _history.prices = buyMovie.Ticket * buyMovie.amount;
                _history.date_buy = DateTime.Now;
                db.histories.Add(_history);
                Member member = db.Members.Find(_history.id_member);
                member.Money -= _history.prices;
                Session["money"] = member.Money;
                db.Entry(member).State = System.Data.Entity.EntityState.Modified;
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
                    member1.status = false;
                    db.Members.Add(member1);
                    db.SaveChanges();
                    var id = db.Members.Where(e => e.Username == member.Username).FirstOrDefault().ID;
                    IsVerified(member.Email,member.Username,id);
                    return RedirectToAction("Message");
                    
                }
            }
            return View(member);
        }
        public ActionResult Message()
        {
            return View();
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
            var IsVerified = db.Members.Where(e => e.Username == member.Username).FirstOrDefault();
            if (IsVerified ==null)
            {
                ViewBag.AccountMessage = "Tài khoản hoặc mật khẩu sai";
                return View();
            }
            member.Password = Mahoa(member.Password);
            if ((db.Members.Count(e => e.Username == member.Username && e.Password == member.Password) > 0))
            {
                Session["id_member"] = db.Members.Where(e => e.Username == member.Username).FirstOrDefault().ID;
                Session["name_member"] = db.Members.Where(e => e.Username == member.Username).FirstOrDefault().Username;
                Session["money"] = db.Members.Where(e => e.Username == member.Username).FirstOrDefault().Money;
                return RedirectToAction("Index");
            }
            else if (IsVerified.status == false)
            {
                ViewBag.AccountMessage = "Tài khoản chưa được kích hoạt";
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
            int.TryParse(Session["id_member"].ToString(), out id_member);
            return View(db.histories.Where(e => e.id_member == id_member));
        }

        public ActionResult ForgotPasswrod()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPasswrod(string email)
        {
            Member member = db.Members.Where(e => e.Email == email).FirstOrDefault();
            if (member == null)
            {
                return View();
            }
            SendMail(email, member.Username);
            return RedirectToAction("ChangePasswrod",new { email });
        }
        public ActionResult ChangePasswrod(string email)
        {
            MenberChangePassword menber = new MenberChangePassword();
            menber.email = email;
            return View(menber);
        }
        [HttpPost]
        public ActionResult ChangePasswrod(string password, string email)
        {
            try
            {
                Member member = db.Members.Where(e => e.Email == email).FirstOrDefault();
                member.Password = Mahoa(password);
                db.Entry(member).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public ActionResult About()
        {
            return View();
        }
        //tao key đóa
        private string CreateKey()
        {
            Random random = new Random();
            do
            {
                StringBuilder key = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {

                    int number = random.Next(0, 2);
                    switch (number)
                    {
                        case 0:
                            key.Append(random.Next(0, 9));
                            break;
                        case 1:
                            key.Append(Convert.ToChar(random.Next(65, 90)));
                            break;
                        case 2:
                            key.Append(Convert.ToChar(random.Next(97, 122)));
                            break;
                        default:
                            break;
                    }
                }
                string key_ = key.ToString();
                return key.ToString();

            } while (true);
        }
        //gửi mail đóa
        private void SendMail(string to, string name)
        {
            string code = CreateKey();
            Session["code"] = code;
            string htmlMail;
            using (StreamReader reader = new StreamReader(HttpContext.Server.MapPath("~/Views/MailTemplate.html")))
            {               
                htmlMail = reader.ReadToEnd();
                htmlMail = htmlMail.Replace("{name}", name);
                htmlMail = htmlMail.Replace("{code}", code);
            }
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.googlemail.com");

                mail.From = new MailAddress("khoado29k11@viendong.edu.vn");
                mail.To.Add(to);
                mail.IsBodyHtml = true;
                mail.Subject = "Quên mật khẩu";
                mail.Body = htmlMail;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("khoado29k11@viendong.edu.vn", "khoa958632147");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        //Xác nhận email 
        private void IsVerified(string to, string name, int id )
        {
            string htmlMail;
            using (StreamReader reader = new StreamReader(HttpContext.Server.MapPath("~/VerifiedEmail.html")))
            {
                htmlMail = reader.ReadToEnd();
                htmlMail = htmlMail.Replace("{name}", name);
                htmlMail = htmlMail.Replace("{link", Url.Action("VerifiedMail", "Home",new {id}, "https"));
            }
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.googlemail.com");

                mail.From = new MailAddress("khoado29k11@viendong.edu.vn");
                mail.To.Add(to);
                mail.IsBodyHtml = true;
                mail.Subject = "Xác nhận email";
                mail.Body = htmlMail;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("khoado29k11@viendong.edu.vn", "khoa958632147");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public ActionResult VerifiedMail(int id )
        {
            Member member = db.Members.Find(id);
            if (member.status == true)
            {
                return RedirectToAction("Index");
            }
            member.status = true;
            db.Entry(member).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View();
        }
    }
}
