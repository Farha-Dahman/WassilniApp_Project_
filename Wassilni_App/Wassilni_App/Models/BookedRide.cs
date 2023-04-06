using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.Models
{
    public class BookedRide
    {

        private string _bookedRideId;
        private string _rideId;
        private string _userId;
        private string _driverId;
        private string _driverName;
        private DateTime _bookedDate;
        private int _numberOfSeatsBooked;
        private string _startLocation;
        private string _endLocation;
        private DateTime _date;
        private DateTime _time;
        private User _passengers;

        public string BookedRideId
        {
            get { return _bookedRideId; }
            set { _bookedRideId = value; }
        }

        public string RideId
        {
            get { return _rideId; }
            set { _rideId = value; }
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string DriverId
        {
            get { return _driverId; }
            set { _driverId = value; }
        }

        public string DriverName
        {
            get { return _driverName; }
            set { _driverName = value; }
        }

        public DateTime BookedDate
        {
            get { return _bookedDate; }
            set { _bookedDate = value; }
        }

        public int NumberOfSeatsbooked
        {
            get { return _numberOfSeatsBooked; }
            set { _numberOfSeatsBooked = value; }
        }

        public string StartLocation
        {
            get { return _startLocation; }
            set { _startLocation = value; }
        }

        public string EndLocation
        {
            get { return _endLocation; }
            set { _endLocation = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public User Passengers
        {
            get { return _passengers; }
            set { _passengers = value; }
        }

    }
}
