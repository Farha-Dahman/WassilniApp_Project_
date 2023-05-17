using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Org.Xmlpull.V1.Sax2;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class RideViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");

        public ICommand RequestRideCommand { get; set; }
        public ICommand OnItemTapped { get; set; }


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
        public DateTime Date { get; set; }

        public String TripDate { get; set; }
        public List<Rider> Riders { get; set; }

        public TimeSpan TripTime { get; set; }

        public RideViewModel(Ride ride)
        {

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
            TripDate = ride.Date.Date.ToString("yyyy-MM-dd");
            Riders = ride.Riders;

            OnItemTapped = new Command<string>(OnFrameClicked);


        }

        private async void OnFrameClicked(string data)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TripDetailsPage(data));
        }

    }
} 

