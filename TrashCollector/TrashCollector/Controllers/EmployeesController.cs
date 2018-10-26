﻿using Microsoft.AspNet.Identity;
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

        public ActionResult Index(/*string day*/int? debuggingProperty) // This feels very anti-Polymorphism
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
            switch (debuggingProperty)
            {
                case 1:
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))

                        //Both of these give an empty list
                        && !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd)
                        && !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        //&& DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd
                        //&& DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart
                        ).
                        ToList());
                case 2:
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))

                        //Both of these give an empty list
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd)
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        && DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd
                        && DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart
                        ).
                        ToList());
                case 3: // Inverted > < signs
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))

                        && !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd)
                        && !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        //&& DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd
                        //&& DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart
                        ).
                        ToList());
                case 4: // Inverted > < signs
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                            
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd)
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        && DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd
                        && DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart
                        ).
                        ToList());
                case 5: // use HasValue
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                            
                        && ((!(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd)
                        && !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart)) || !(!c.SuspendServiceEnd.HasValue && !c.SuspendServiceStart.HasValue))

                        //&& DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd
                        //&& DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart
                        ).
                        ToList());
                case 6: // use HasValue
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                            
                        //&& ((!(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd)
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart)) || (!c.SuspendServiceEnd.HasValue && !c.SuspendServiceStart.HasValue))

                        && 
                        (
                            (DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd && DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart)
                            || (!c.SuspendServiceEnd.HasValue && !c.SuspendServiceStart.HasValue))
                        ).
                        ToList());
                case 7: // Inverted > < signs give the opposite of what I want, so drop that ! operator like its hot
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                            
                        && !(!(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd)
                        && !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart)) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        //&& DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd
                        //&& DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart
                        ).
                        ToList());
                case 8: // Inverted > < signs
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                            
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd)
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        && !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd
                        && DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart)
                        ).
                        ToList());
                case 9: // Inverted > < signs give the opposite of what I want, so drop that ! operator like its hot
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                            
                        && (!(!(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd)
                        && !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart)) || (!c.SuspendServiceEnd.HasValue && !c.SuspendServiceStart.HasValue)) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        //&& DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd
                        //&& DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart
                        ).
                        ToList());
                case 10: // Inverted > < signs
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))
                            
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd)
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        && (!(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd
                        && DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart) || (!c.SuspendServiceEnd.HasValue && !c.SuspendServiceStart.HasValue))
                        ).
                        ToList());
                default:
                    return View(db.Customers.
                        Where
                        (
                            c => c.Zipcode == currentEmployee.AssignedZipcode
                            && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))

                        //Both of these give an empty list
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd)
                        //&& !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                        //&& DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd
                        //&& DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart
                        ).
                        ToList());
            }


            return View(db.Customers.
                Where
                (
                    c => c.Zipcode == currentEmployee.AssignedZipcode
                    && (c.PickupDay == currentDayAsAString || DbFunctions.TruncateTime(c.ExtraPickup) == DbFunctions.TruncateTime(DateTime.Now))

                //Both of these give an empty list
                //&& !(DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceEnd)
                //&& !(DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceStart) // Use ! because c.SuspendServiceEnd/Start is nullable. Now < null is false and Now > null is false.

                //&& DbFunctions.TruncateTime(DateTime.Now) > c.SuspendServiceEnd
                //&& DbFunctions.TruncateTime(DateTime.Now) < c.SuspendServiceStart
                ).
                ToList());
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
