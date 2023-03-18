using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.CustomerModel;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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

        public ActionResult Admin()
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var customers = new List<Customer>();
                foreach (Insuree insuree in db.Insurees)
                {
                    var customer = new Customer();
                    customer.FirstName = insuree.FirstName;
                    customer.LastName = insuree.LastName;
                    customer.EmailAddress = insuree.EmailAddress;
                    customer.DateOfBirth = insuree.DateOfBirth;
                    customer.CarYear = insuree.CarYear;
                    customer.CarMake = insuree.CarMake;
                    customer.CarModel = insuree.CarModel;
                    customer.SpeedingTickets = insuree.SpeedingTickets;
                    customer.DUI = insuree.DUI;
                    customer.CoverageType = insuree.CoverageType;
                    customers.Add(customer);
                }

                foreach (Customer customer in customers)
                {
                    var Base = 50.0m;
                    var Age = DateTime.Now.Year - customer.DateOfBirth.Year;
                    var AgeAllowance = 25.0m;
                    if (Age < 18) AgeAllowance = 100.0m;
                    else if (Age > 19 && Age < 25) AgeAllowance = 50.0m;
                    var AdditionalforCarYear = 0.0m;
                    if (customer.CarYear < 2000) AdditionalforCarYear = 25.0m;
                    else if( customer.CarYear > 2015) AdditionalforCarYear = 25.0m; 
                    var CarMakeAdditionalPrice = 0.0m;
                    if (customer.CarMake == "Porshe") CarMakeAdditionalPrice = 25.0m;
                    var CarModelAdditionalPrice = 0.0m;
                    if (customer.CarModel == "Carrera") CarModelAdditionalPrice = 25.0m;
                    var PriceforSpeedingTikects = 0.0m;
                    if (customer.SpeedingTickets > 0) PriceforSpeedingTikects = 10.0m * customer.SpeedingTickets;
                    customer.Quote = 12 * (Base + AgeAllowance + AdditionalforCarYear + PriceforSpeedingTikects) + CarMakeAdditionalPrice + CarModelAdditionalPrice;
                    if (customer.DUI == false) customer.Quote = customer.Quote * 1.25m;
                    if (customer.CoverageType == false) customer.Quote = customer.Quote * 1.5m;
                   
                }
                return View(customers);
            }


                   
        }
    }
}
