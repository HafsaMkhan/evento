using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Login(int id=0)
        {
            AdminViewModel ad = new AdminViewModel();
            return View(ad);
        }
        [HttpPost]
        public ActionResult Login(AdminViewModel ad)
        {
            using (EventoEntities db = new EventoEntities())
            {
                if (db.AdminDbs.Any(x => x.username == ad.username && x.password == ad.password))
                {
                    ViewBag.SuccessMessage = "Login Successful!";
                    return View("RegisteredCompanies", new List<WebApplication4.Models.CompanyDb>());
                }
                ViewBag.SucceessMessage = "Incorrect Email or Password!";
                return View("Login", new AdminViewModel());
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisteredCompanies()
        {
            EventoEntities db = new EventoEntities();
            List<CompanyDb> rc = db.CompanyDbs.ToList();
            return View(rc);
        }

        public ActionResult Users()
        {
            EventoEntities db = new EventoEntities();
            List<UserDb> ru = db.UserDbs.ToList();
            return View(ru);
        }

        public ActionResult CompanyRequests()
        {
            EventoEntities db = new EventoEntities();
            List<CompanyDb> rc = db.CompanyDbs.ToList();
            return View(rc);
        }

        public ActionResult Feedback()
        {
            EventoEntities db = new EventoEntities();
            List<FeedbackDb> rc = db.FeedbackDbs.ToList();
            return View(rc);
        }
        [HttpGet]
        public ActionResult ResetPassword(int id=0)
        {
            AdminViewModel ad = new AdminViewModel();
            return View(ad);
        }
        [HttpPost]
        public ActionResult ResetPassword(AdminViewModel adm)
        {
            using (EventoEntities db = new EventoEntities())
            {
                if (adm.change_password == adm.confirm_password)
                {
                    if (db.AdminDbs.Any(x => x.password == adm.password))
                    {
                        AdminDb ad = new AdminDb();
                        ad.password = adm.change_password;
                        
                        ViewBag.SuccessMessage = "Password changed successfully!";
                        return View("ResetPassword", new AdminViewModel());
                    }
                    ViewBag.SucceessMessage = "Incorrect Password!";
                    return View("ResetPassword", new AdminViewModel());
                }
                else
                {
                    ViewBag.Message = "Password doesnot match!";
                }
                
            }
            return View();
        }
    }
}