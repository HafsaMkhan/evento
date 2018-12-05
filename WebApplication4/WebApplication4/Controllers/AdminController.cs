using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using WebApplication4.Models;
using System.Threading.Tasks;
using System.Web.Security;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Login(int id=0)
        {
            AdminViewModel ad = new AdminViewModel();
            return View(ad);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginUser", "User");
        }

        [HttpPost]
        public ActionResult Login(AdminViewModel login, string ReturnUrl = "")
        {
            string message = "";
            using (EventoEntities db = new EventoEntities())
            {
                var v = db.AdminDbs.Where(x => x.username == login.username && x.password == login.password).FirstOrDefault();
                if (v != null)
                {
                    int timeout = login.RememberMe ? 525600 : 20; 
                    var ticket = new FormsAuthenticationTicket(login.username, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("RegisteredCompanies", "Admin");
                    }
                }
                else
                {
                    message = "Incorrect Email or Password!";
                }
                ViewBag.Message = message;
                return View();
            }
        }

        [Authorize]
        public ActionResult RegisteredCompanies()
        {
            EventoEntities db = new EventoEntities();
            List<CompanyDb> rc = db.CompanyDbs.ToList();
            return View(rc);
        }

        [Authorize]
        public ActionResult Users()
        {
            EventoEntities db = new EventoEntities();
            List<UserDb> ru = db.UserDbs.ToList();
            return View(ru);
        }

        [Authorize]
        public ActionResult CompanyRequests()
        {
            EventoEntities db = new EventoEntities();
            List<CompanyDb> rc = db.CompanyDbs.ToList();
            return View(rc);
        }

        [Authorize]
        public ActionResult Feedback()
        {
            EventoEntities db = new EventoEntities();
            List<FeedbackDb> rc = db.FeedbackDbs.ToList();
            return View(rc);
        }

        [Authorize]
        public ActionResult ResetPassword(string id)
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ResetPassword(AdminResetViewModel model)
        {
            string message = "";
            bool status = false;
            if (ModelState.IsValid)
            {
                using (EventoEntities db = new EventoEntities())
                {
                    var ad = db.AdminDbs.Where(a => a.username == "admin" && a.password == model.OldPassword).FirstOrDefault();
                    if (ad != null)
                    {
                        ad.password = model.NewPassword;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        return RedirectToAction("RegisteredCompanies");
                    }
                    else
                    {
                        message = "Old Password is incorrect.";
                        status = true;
                    }
                }
            }
            ViewBag.Status = status;
            ViewBag.Message = message;
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteUser(string id1)
        {
            using(EventoEntities db= new EventoEntities())
            {
                if (id1 == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserDb user = db.UserDbs.Find(id1);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        [HttpPost,ActionName("DeleteUser")]
        public ActionResult DeleteConfirmed(string id1)
        {
            using (EventoEntities db = new EventoEntities())
            {
                UserDb user = db.UserDbs.Where(a => a.Email == id1).FirstOrDefault();
                db.UserDbs.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Users");
        }

        


    }
}