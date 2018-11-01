using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text;

namespace TrashCollector.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        

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

        [Display(Name = "Extra Pickup Date")]
        public DateTime? ExtraPickup { get; set; }

        [Display(Name = "Suspended Service Start Date")]
        public DateTime? SuspendServiceStart { get; set; }

        [Display(Name = "Suspended Service End Date")]
        public DateTime? SuspendServiceEnd { get; set; }

        public DateTime? LastTimeTrashWasPickedUp { get; set; }
        
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }

        public async void updateLatitudeAndLongitudeAsync()
        {
            float[] newData = await GetLatitudeAndLongitude();
            Latitude = newData[0];
            Longitude = newData[1];
        }
        private async Task<float[]> GetLatitudeAndLongitude()
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + Address + Zipcode.ToString() + "&key=" + APIKeys.geocodeKey;

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Geocode result = await response.Content.ReadAsAsync<Geocode>();
                    //Geocode result = await response.Content.Read
                    return new float[] { result.Results.Geometry.Location.Lat, result.Results.Geometry.Location.Lng };
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private class Geocode
        {
            public GeocodeResult Results { get; set; }
        }

        private class GeocodeResult
        {
            public GeocodeResultGeometry Geometry { get; set; }
        }

        private class GeocodeResultGeometry
        {
            public GeocodeResultGeometryLocation Location { get; set; }
        }

        private class GeocodeResultGeometryLocation
        {
            public float Lat { get; set; }
            public float Lng { get; set; }
        }
    }

    
}