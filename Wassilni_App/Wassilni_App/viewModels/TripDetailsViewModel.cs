﻿using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.views;
using Xamarin.Essentials;
using Xamarin.Forms;

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
        //private DateTime _pickupDateTime;
        private string _Date;
        private TimeSpan _Time;
        private string _startLocation;
        private string _endLocation;
        private string _riderName;
        private string _riderPhotoUrl;

        private Rider _rides;
        public Rider Rides
        {
            get { return _rides; }
            set { _rides = value; }
        }
        public String RiderPhotoUrl
        {
            get { return _riderPhotoUrl; }
            set { SetProperty(ref _riderPhotoUrl, value); }
        }
        public String RiderName
        {
            get { return _riderName; }
            set { SetProperty(ref _riderName, value); }
        }

        public ICommand RequestRideCommand { get; set; }


        public String StartLocation
        {
            get { return _startLocation; }
            set { SetProperty(ref _startLocation, value); }
        }
        public String EndLocation
        {
            get { return _endLocation; }
            set { SetProperty(ref _endLocation, value); }
        }
        public String PhotoUrl
        {
            get { return _photoUrl; }
            set { SetProperty(ref _photoUrl, value); }
        }
        public String DriverName
        {
            get { return _driverName; }
            set { SetProperty(ref _driverName, value); }
        }
        public int Number_of_seats
        {
            get { return _numberOfSeats; }
            set { SetProperty(ref _numberOfSeats, value); }
        }
        public String CarModel
        {
            get { return _carModel; }
            set { SetProperty(ref _carModel, value); }
        }

        public decimal PricePerRide 
        {
            get { return _pricePerRide; }
            set { SetProperty(ref _pricePerRide, value); }
        }
        public string TripDate  
        {
            get { return _Date; }
            set { SetProperty(ref _Date, value); }  
        }
        public TimeSpan TripTime 
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value); }
        }
        public string PhoneNumber 
        { 
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }


        public TripDetailsViewModel(string rideId)
        {

            GetRide(rideId);

        }

        public ObservableCollection<Rider> Riders { get; set; } = new ObservableCollection<Rider>();
        public bool IsLabelVisible { get;  set; }

        public async Task GetRide(string rideId)
        {
            var ride = await firebaseClient.Child("Ride").Child(rideId).OnceSingleAsync<Models.Ride>();
            if (ride != null)
            {
                TripDate = ride.TripDate;
                TripTime = ride.TripTime;
                PhotoUrl = ride.PhotoUrl;
                DriverName = ride.DriverName;
                StartLocation = ride.StartLocation;
                EndLocation = ride.EndLocation;
                Number_of_seats = ride.Number_of_seats;
                CarModel = ride.CarModel;
                PricePerRide = ride.PricePerRide;
                PhoneNumber = ride.PhoneNumber;
                if (ride.Riders != null)
                {
                    Riders.Clear();
                    if (ride.Riders.Count() == 1)
                    {
                        // myLabel.Visible = true;
                        IsLabelVisible = true;
                    }
                    else
                    {
                        foreach (var rider in ride.Riders.Skip(1))
                        {
                            Riders.Add(rider);
                            RiderName = rider.RiderName;
                            RiderPhotoUrl = rider.RiderPhotoUrl;
                        }
                    }
                }
            }
        }




    }
}
