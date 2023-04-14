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
        private string _riderName;
        private string _riderId;
        public string RiderID
        {
            get { return _riderId; }
            set { _riderId = value; }
        }
        public string RiderName
        {
            get { return _riderName; }
            set { _riderName = value; }
        }
        public string BookedRideId
        {
            get { return _bookedRideId; }
            set { _bookedRideId = value; }
        }
        public DateTime PickupDateTime { get; set; }
        public string CarModel { get; set; }
        public string RideID
        {
            get { return _rideId; }
            set { _rideId = value; }
        }
        public string UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string DriverID
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
        public int Number_of_seats
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
        public string PhotoUrl { get; set; }
        public decimal PricePerRide { get; set; }
        public string TripID { get; set; }
        public List<Rider> Riders { get; set; }
    }
    public class Rider
    {
        public string RiderID { get; set; }
        public string RiderName { get; set; }
        public string RiderPhotoUrl { get; set; }
        public string Gender { get; set; }
        public int Number_of_seats { get; set; }
    }

}
