using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.viewModels
{
    internal class MyTripsViewModel
    {

        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public string DriverName { get; set; }
        public string DriverId { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public String TripDate { get; set; }
        public TimeSpan TripTime { get; set; }
        public string CarDetails { get; set; }


    }
}
