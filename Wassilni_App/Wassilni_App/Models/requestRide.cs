using System;
using System.Collections.Generic;
using System.Text;

namespace Wassilni_App.Models
{
    public class requestRide
    {
        private string _requestId;
        private string _riderId;
        private string _driverId;
        private string _rideId;
        private DateTime _requestDate;
        private string _startLocation;
        private string _endLocation;
        private bool _isAccepted;

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
