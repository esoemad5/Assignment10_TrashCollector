using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Zipcode { get; set; }
        [Display(Name = "Money Owed")]
        public float MoneyOwed { get; set; }
        [Display(Name = "Pickup Day")]
        public string PickupDay { get; set; }
        [Display(Name = "Extra Pickup")]
        public DateTime ExtraPickup { get; set; }
        [Display(Name = "Suspended Service Start Date")]
        public DateTime SuspendServiceStart { get; set; }
        [Display(Name = "Suspended Service End Date")]
        public DateTime SuspendServiceEnd { get; set; }
    }
}