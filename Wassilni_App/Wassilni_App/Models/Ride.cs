﻿using System;
using System.Collections.Generic;
using System.Text;
namespace Wassilni_App.Models
{
    public class Ride
    {
        public string RideID { get; set; }
        public string DriverID { get; set; }
        public string DriverName { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public decimal PricePerRide { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TripTime { get; set; }
        public DateTime PickupDateTime { get; set; }
        public int Number_of_seats { get; set; }
        public string CarModel { get; set; }
        public string PhotoUrl { get; set; }
        public string TripDate { get; set; }


        public User Users { get; set; }
    }
}