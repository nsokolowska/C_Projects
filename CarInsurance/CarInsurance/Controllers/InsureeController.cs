using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                insuree.Quote = GetQuote(insuree);
                db.SaveChanges();
                return RedirectToAction("Quote", insuree);
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
            return View();
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
                    
            return View(db.Insurees.ToList());

        }
        public decimal GetQuote(Insuree insuree)
        {
            
            var Base = 50.0m;
            var Age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            var AgeAllowance = 25.0m;
            if (Age < 18) AgeAllowance = 100.0m;
            else if (Age > 19 && Age < 25) AgeAllowance = 50.0m;
            var AdditionalforCarYear = 0.0m;
            if (insuree.CarYear < 2000) AdditionalforCarYear = 25.0m;
            else if (insuree.CarYear > 2015) AdditionalforCarYear = 25.0m;
            var CarMakeAdditionalPrice = 0.0m;
            if (insuree.CarMake == "Porshe") CarMakeAdditionalPrice = 25.0m;
            var CarModelAdditionalPrice = 0.0m;
            if (insuree.CarModel == "Carrera") CarModelAdditionalPrice = 25.0m;
            var PriceforSpeedingTikects = 0.0m;
            if (insuree.SpeedingTickets > 0) PriceforSpeedingTikects = 10.0m * insuree.SpeedingTickets;
            insuree.Quote = 12 * (Base + AgeAllowance + AdditionalforCarYear + PriceforSpeedingTikects) + CarMakeAdditionalPrice + CarModelAdditionalPrice;
            if (insuree.DUI == true) insuree.Quote *= 1.25m;
            if (insuree.CoverageType == true) insuree.Quote *= 1.5m;
            return insuree.Quote;
        }

        public ActionResult Quote(Insuree insuree)
        {
            
            return View(insuree);
            
        }
    }
}
