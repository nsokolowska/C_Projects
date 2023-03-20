using NewsletterApp.Models;
using NewsletterAppMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMvc.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                var signups = db.SignUps.Where(x => x.Removed == null).ToList();
                //var signups = (from c in db.SignUps
                //               where c.Removed == null
                //               select c).ToList();
                var signupvms = new List<SignupVm>();
                foreach (var signup in signups)
                {
                    var signupvm = new SignupVm();
                    signupvm.Id = signup.Id;
                    signupvm.FirstName = signup.FirstName;
                    signupvm.LastName = signup.LastName;
                    signupvm.EmailAddress = signup.EmailAddress;
                    signupvms.Add(signupvm);
                }
                return View(signupvms);
            }


        }
        public ActionResult Unsubscribe(int Id)
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                var signup = db.SignUps.Find(Id);
                signup.Removed = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}