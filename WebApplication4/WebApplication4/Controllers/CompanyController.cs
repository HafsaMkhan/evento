using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class CompanyController : Controller
    {
        [HttpGet]
        public ActionResult RegisterCompany(int id = 0)
        {
            CompanyViewModel cb = new CompanyViewModel();
            return View(cb);
        }
        [HttpPost]
        public ActionResult RegisterCompany(CompanyViewModel c)
        {
            using (EventoEntities db = new EventoEntities())
            {
                if (c.Password == c.C_Password)
                {
                    CompanyDb cd = new CompanyDb();
                    if (cd.Email == c.Email && cd.Name == c.Name)
                    {
                        ViewBag.SuccessMessage = "Already Exists";
                    }
                    else
                    {
                        cd.Name = c.Name;
                        cd.Email = c.Email;
                        cd.Contact = c.Contact;
                        cd.Ceo = c.Ceo;
                        cd.Password = c.Password;
                        cd.C_Password = c.C_Password;
                        cd.Address = c.Address;
                        db.CompanyDbs.Add(cd);
                        db.SaveChanges();
                        ModelState.Clear();
                        ViewBag.SuccessMessage = "You've have been successfully!";
                    }
                    
                }
                else { ViewBag.SuccessMessage("Password and Confirm Password doesnot match"); }
            }
            return View("RegisterCompany", new CompanyViewModel());
        }

        [HttpGet]
        public ActionResult LoginCompany(int id = 0)
        {
            CompanyViewModel cb = new CompanyViewModel();
            return View(cb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCompany(CompanyViewModel comp)
        {
            using (EventoEntities db = new EventoEntities())
            {
                if (db.CompanyDbs.Any(x => x.Email == comp.Email && x.Password == comp.Password))
                {
                    ViewBag.SuccessMessage = "Login Successful!";
                    return View("Home", new CompanyViewModel());
                }
                ViewBag.SucceessMessage = "Incorrect Email or Password!";
                return View("LoginCompany", new CompanyViewModel());
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult AddPackages()
        {
            return View();
        }
    }
}