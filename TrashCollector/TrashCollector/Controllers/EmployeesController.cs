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

        // GET: Employees
        //public ActionResult Index()
        //{
        //    string currentUserID = User.Identity.GetUserId();
        //    Employee currentEmployee = db.Employees.Where(e => e.UserID == currentUserID).Single();
        //    return View(db.Customers.Where(c=> c.Zipcode == currentEmployee.AssignedZipcode).ToList());
        //}

        public ActionResult Index(/*string day*/) // This feels very anti-Polymorphism (monomorphism?)
        {
            string currentUserID = User.Identity.GetUserId();
            Employee currentEmployee = db.Employees.Where(e => e.UserID == currentUserID).First();

            //Test();

            //if (string.IsNullOrEmpty(day))
            //{
            //    return View(db.Customers.Where(c=> c.Zipcode == currentEmployee.AssignedZipcode).ToList());
            //}

            //DateTime firstSunday = new DateTime(1753, 1, 7); An idea I decided not to use
            string currentDayAsAString = DateTime.Now.DayOfWeek.ToString();

            return View(db.Customers.
                Where
                (
                    // Only customers in the employee's zipcode
                    c => c.Zipcode == currentEmployee.AssignedZipcode
                    // Customers who routinely have their trash picked up today
                    && (
                        c.PickupDay == currentDayAsAString
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
                ToList());
        }

        public void VisitCustomer()
        {

        }

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
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
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

            return View(employee);
        }
        private void Test()
        {
            //Employee testEmployee = new Employee();
            //testEmployee.AssignedZipcode = 99999;
            //testEmployee.FirstName = "test";
            //testEmployee.LastName = "Employee";
            //db.Employees.Add(testEmployee);
            Customer testCustomer = new Customer();
            testCustomer.FirstName = "QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ";
            testCustomer.LastName = "QQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ";
            testCustomer.Zipcode = 90210;
            db.Customers.Add(testCustomer);

            //db.SaveChanges();
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
            return View(employee);
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
            return View(employee);
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
            return View(employee);
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
