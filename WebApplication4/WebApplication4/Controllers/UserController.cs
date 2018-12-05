using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        [HttpGet]
        public ActionResult RegisterUser(int id = 0)
        {
            UserViewModel user = new UserViewModel();
            return View(user);
        }

        [HttpPost]
        public ActionResult RegisterUser([Bind(Exclude = "IsEmailVerified,ActivationCode")] UserViewModel user)
        {
            bool Status = false;
            string message = "";
            using (EventoEntities db = new EventoEntities())
            {
                if (ModelState.IsValid)
                {
                    if (user.Password == user.C_Password)
                    {
                        var v = db.UserDbs.Where(a => a.Email == user.Email).FirstOrDefault();
                        if (v!=null)
                        {
                            ModelState.AddModelError("EmailExist", "Email already exist");
                            return View(user);
                        }
                        else
                        {
                            #region Generate Activation Code
                            user.ActivationCode = Guid.NewGuid();
                            #endregion

                            user.IsEmailVerified = false;

                            UserDb us = new UserDb();
                            us.Name = user.Name;
                            us.Email = user.Email;
                            us.Password = user.Password;
                            us.ActivationCode = user.ActivationCode;
                            db.UserDbs.Add(us);
                            db.SaveChanges();
                            SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                            ModelState.Clear();
                            message = "You've have been successfully registered!";
                            Status = true;
                            return RedirectToAction("LoginUser", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordMatch", "Password does not match!");
                        return View(user);
                    }
                }
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View("RegisterUser", new UserViewModel());
        }
        
        [NonAction]
        public void SendVerificationLinkEmail(string EmailId, string activationCode, string emailfor = "VerifyAccount")
        {

            var verifyUrl = "/User/" + emailfor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("evento.managment@gmail.com", "Evento Management System");
            var toEmail = new MailAddress(EmailId);
            var fromEmailPassword = "evento12@#";
            string subject = "", Message ="";
            if (emailfor == "VerifyAccount")
            {
                subject = "Congragulations! Your account has been approved!";
                Message = "We welcome you to get your events planned and executed by Evento and its collaborators. Hope you have a good experience. :)" + "<br/>Please click the link below to verify your account" +
                    "<br/><br/><a href=" + link + ">" + link + "</a>";
            }
            else if(emailfor == "ResetPassword")
            {
                subject = "Reset Password";
                Message = "Hey, <br/><br/>We got request of your account reset password." + "Please click on the link below to reset your password" +
                    "<br/><br/><a href=" + link + ">" + link + "</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = Message,
                IsBodyHtml = true
            })
            smtp.Send(message);
        }

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (EventoEntities db = new EventoEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var c = db.UserDbs.Where(u => u.ActivationCode == new Guid(id)).FirstOrDefault();
                if (c != null)
                {
                    c.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request!";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        [HttpGet]
        public ActionResult LoginUser(int id = 0)
        {
            UserViewModel cb = new UserViewModel();
            return View(cb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(UserViewModel login, string ReturnUrl="")
        {
            string message = "";
            using (EventoEntities db = new EventoEntities())
            {
                var v = db.UserDbs.Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();
                if (v!=null)
                {
                    int timeout = login.RememberMe ? 525600 : 20;
                    var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
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
                        return RedirectToAction("Home", "User");
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
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginUser", "User");
        }
        

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string message = "";
            bool Status = false;
            using(EventoEntities db =new EventoEntities())
            {
                var account = db.UserDbs.Where(a => a.Email == EmailID).FirstOrDefault();
                if(account != null)
                {
                    //Send email reset password mail
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(EmailID, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    ModelState.Clear();
                    message = "Reset passord lsink has been sent to your account";
                }
                else
                {
                    message = "Invalid Account";
                }
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            using(EventoEntities db=new EventoEntities())
            {
                var user = db.UserDbs.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if(user!=null)
                {
                    ResetPasswordViewModel model = new ResetPasswordViewModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using(EventoEntities db =new EventoEntities())
                {
                    var user = db.UserDbs.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        user.ResetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        ModelState.Clear();
                        message = "Password has been changed successfully!";
                    }
                }
            }
            else
            {
                message = "Something Inavlid!";
            }
            ViewBag.Message = message;
            return View(model);
        }

        [Authorize]
        public ActionResult Home()
        {
            EventoEntities db = new EventoEntities();
            List<PackageDb> pd = db.PackageDbs.ToList();
            return View(pd);
        }



    }
}