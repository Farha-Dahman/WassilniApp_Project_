using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Xamarin.Essentials;
using static Java.Util.Jar.Attributes;

namespace Wassilni_App.viewModels
{
    public class TripDetailsViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");


        private string _photoUrl;
        private string _driverName;
        private int _numberOfSeats;
        private string _carModel;
        private string _phoneNumber;
        private decimal _pricePerRide;
        private DateTime _pickupDateTime;
        private string _Date;
        private TimeSpan _Time;

        private ICommand _requestRideCommand;

        // Properties
        public string PhotoUrl
        {
            get => _photoUrl;
            set
            {
                _photoUrl = value;
                OnPropertyChanged();
            }
        }

        public string DriverName
        {
            get => _driverName;
            set
            {
                _driverName = value;
                OnPropertyChanged();
            }
        }

        public int Number_of_seats
        {
            get => _numberOfSeats;
            set
            {
                _numberOfSeats = value;
                OnPropertyChanged();
            }
        }

        public string CarModel
        {
            get => _carModel;
            set
            {
                _carModel = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public decimal PricePerRide
        {
            get => _pricePerRide;
            set
            {
                _pricePerRide = value;
                OnPropertyChanged();
            }
        }

        public DateTime PickupDateTime
        {
            get => _pickupDateTime;
            set
            {
                _pickupDateTime = value;
                OnPropertyChanged();
            }
        }

        public string TripDate
        {
            get => _Date;
            set
            {
                _Date = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan TripTime
        {
            get => _Time;
            set
            {
                _Time = value;
                OnPropertyChanged();
            }
        }


        public TripDetailsViewModel(string rideId)
        {
            rideId = Preferences.Get("RideId", string.Empty);
            GetRide(rideId);
        }

        public async Task GetRide(string rideId)
        {
            var ride = await firebaseClient.Child("Ride").Child(rideId).OnceSingleAsync<Models.Ride>();
            if (ride != null)
            {
                PhotoUrl = ride.PhotoUrl;
                DriverName = ride.DriverName;
                Number_of_seats = ride.Number_of_seats;
                CarModel = ride.CarModel;
                PricePerRide = ride.PricePerRide;
                TripDate = ride.TripDate;
                TripTime = ride.TripTime;
                PhoneNumber = "0" + ride.PhoneNumber;
            }
        }


    }
}
