using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Org.Apache.Http.Protocol;
using Org.Xmlpull.V1.Sax2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace Wassilni_App.viewModels
{
    public class RideViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");

        public string RideId { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public decimal PricePerRide { get; set; }
        public int Number_of_seats { get; set; }
        public string RiderName { get; set; }
        public string DriverName { get; set; }
        public string DriverId { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string Date { get; set; }
        public TimeSpan TripTime { get; set; }

        // Your RideViewModel properties and methods...
        public RideViewModel(Ride ride)
        {
            // Set the properties using the provided ride object
            DriverName = ride.DriverName;
            StartLocation = ride.StartLocation;
            EndLocation = ride.EndLocation;
            PricePerRide = ride.PricePerRide;
            Number_of_seats = ride.Number_of_seats;
            PickupDateTime = ride.PickupDateTime;
            DriverId = ride.DriverID;
            PhoneNumber = ride.PhoneNumber;
            RideId = ride.RideID;
            PhotoUrl = ride.PhotoUrl;
            TripTime = ride.TripTime;
            Date = ride.Date.Date.ToString("yyyy-MM-dd");
        }


    } 
}
