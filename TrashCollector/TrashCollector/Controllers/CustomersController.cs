using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            return View("Index", db.Customers.Where(c => c.UserID == userID).ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            /* https://maps.googleapis.com/maps/api/js?key=OUR_API_KEY_GOES_HERE&callback=CALLBACK_FUNCTION
             * The callback at the end can be anything, I changed it to mapFunction
             * Javascript does NOT change the '&' to '&amp;' thankfully. not sure what to do if that becomes the case.
             */
            ViewBag.mapsCall = APIKeys.mapsCall;


            /* Full geocode API call is: geocodeBase + Address + "&key=YOUR_API_KEY_GOES_HERE"
             * https://maps.googleapis.com/maps/api/geocode/json?address=ADDRESS_GOES_HERE&key=YOUR_API_KEY_GOES_HERE
             * 
             * This doesnt work because javascript changes the '&' to '&amp;'
             * ViewBag.geocodeCall = APIKeys.geocodeBase + customer.Address + " " + customer.Zipcode + "&key=" + APIKeys.geocodeKey;
             */

            ViewBag.geocodeBase = APIKeys.geocodeBase;
            ViewBag.addressAndZipcode = customer.Address + " " + customer.Zipcode;
            ViewBag.geocodeKey = APIKeys.geocodeKey;


            return View("Details", customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Address,Zipcode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.UserID = User.Identity.GetUserId();
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Create", customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("Edit", customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Address,Zipcode,MoneyOwed,PickupDay,ExtraPickup,SuspendServiceStart,SuspendServiceEnd, LastTimeTrashWasPickedUp")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.UserID = User.Identity.GetUserId(); // The foreign key wouldn't update so I had to do it here instead of in the Bind
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("Delete", customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
    }
}
