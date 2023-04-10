using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.Models
{
    public class RideRequest
    {

        private string _requestId;
        private string _riderId;
        private string _driverId;
        private string _rideId;
        private DateTime _requestDate;
        private string _startLocation;
        private string _endLocation;
        private bool _isAccepted;
        private string _driverName;
        private string _RiderName;
        private DateTime _pickupDateTime;
        private string _photoUrl;
        private DateTime _Date;
        private TimeSpan _Time;

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public TimeSpan Time
        {
            get { return _Time; }
            set { _Time = value; }
        }
        public string PhotoUrl
        {
            get { return _photoUrl; }
            set { _photoUrl = value; }
        }
        public DateTime PickupDateTime
        {
            get { return _pickupDateTime; }
            set { _pickupDateTime = value; }
        }
        public string RiderName
        {
            get { return _RiderName; }
            set { _RiderName = value; }
        }
        public string DriverName
        {
            get { return _driverName; }
            set { _driverName = value; }
        }
        public string RequestID
        {
            get { return _requestId; }
            set { _requestId = value; }
        }
        public string RiderID
        {
            get { return _riderId; }
            set { _riderId = value; }
        }
        public string DriverID
        {
            get { return _driverId; }
            set { _driverId = value; }
        }
        public string RideID
        {
            get { return _rideId; }
            set { _rideId = value; }
        }
        public DateTime RequestDate
        {
            get { return _requestDate; }
            set { _requestDate = value; }
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
        public bool IsAccepted
        {
            get { return _isAccepted; }
            set { _isAccepted = value; }
        }

    }
}