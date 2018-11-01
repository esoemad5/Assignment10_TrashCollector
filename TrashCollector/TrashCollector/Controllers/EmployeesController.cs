using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        


        public ActionResult PickUpTrash(int customerID)
        {
            Customer customer = db.Customers.Where(c => c.ID == customerID).Single();
            customer.LastTimeTrashWasPickedUp = DateTime.Now;
            customer.MoneyOwed += 50;
            db.SaveChanges();
            return RedirectToAction("Index", "Employees"); // Doesn't keep stuff in the url, so refreshing the page won't re-charge the customer
        }

        // GET: Employees
        public ActionResult Index()
        {
            string currentDayAsAString = DateTime.Now.DayOfWeek.ToString();

            return View("Index", GetListOfCustomers(currentDayAsAString));
        }
        [HttpPost]
        public ActionResult Index([Bind(Include = "day")]string day) // This feels very anti-Polymorphism (monomorphism?)
        {


            //Test();

            //if (string.IsNullOrEmpty(day))
            //{
            //    return View(db.Customers.Where(c=> c.Zipcode == currentEmployee.AssignedZipcode).ToList());
            //}

            //DateTime firstSunday = new DateTime(1753, 1, 7); An idea I decided not to use

            //string currentDayAsAString = DateTime.Now.DayOfWeek.ToString();

            return View("Index", GetListOfCustomers(day));
        }

        private List<Customer> GetListOfCustomers (string day)
        {
            string currentUserID = User.Identity.GetUserId();
            Employee currentEmployee = db.Employees.Where(e => e.UserID == currentUserID).First();

            return db.Customers.
                Where
                (
                    // Only customers in the employee's zipcode
                    c => c.Zipcode == currentEmployee.AssignedZipcode
                    // Customers who routinely have their trash picked up today
                    && (
                        c.PickupDay == day
                        // or requested an Extra Pickup
                        || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                    // Exclude customers who have Suspended Service
                    && (
                        !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd && DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart)
                        // but not those who have never requested Suspended Service (since Customer.SuspendedServiceEnd/Start are both nullable)
                        || (!c.SuspendServiceEnd.HasValue && !c.SuspendServiceStart.HasValue))
                    // Exclude customers who have been serviced today
                    && (
                        !c.LastTimeTrashWasPickedUp.HasValue
                        || DbFunctions.TruncateTime(c.LastTimeTrashWasPickedUp) < DbFunctions.TruncateTime(DateTime.Now))
                ).
                ToList();
        }








        /*
         * The following actions are unused. They would be useful for a user with an Admin role, so they have been left in
         */


        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View("Details", employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,AssignedZipcode")] Employee employee)
        {
            employee.UserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                //Test();
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View("Create", employee);
        }
        
        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View("Edit", employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,AssignedZipcode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View("Delete", employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
