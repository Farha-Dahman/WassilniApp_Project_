using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class RequestViewModel : BaseViewModel
    {
        public string RequestId { get; set; }
        public string RiderId { get; set; }
        public string DriverId { get; set; }
        public string RideId { get; set; }
        public DateTime RequestDate { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public bool IsAccepted { get; set; }
        public string DriverName { get; set; }
        public string RiderName { get; set; }

        public int Number_of_seats { get; set; }

        public DateTime PickUpDateTime { get; set; }

        public DateTime StartDate { get; set; }

        public TimeSpan TripTime { get; set; }

        public string PhotoUrl { get; set; }

        public string PhoneNumber { get; set; }


        public decimal PricePerRide { get; set; }


        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DenyRequestCommand { get; set; }

        public ObservableCollection<RideRequest> RequestRides { get; set; }
        private readonly RequestsViewModel _requestsViewModel;
        private readonly DatabaseHelper _databaseHelper;

        public RequestViewModel(RideRequest request, RequestsViewModel requestsViewModel, DatabaseHelper databaseHelper)
        {
            RequestId = request.RequestID;
            RiderId = request.RiderID;
            DriverId = request.DriverID;
            RideId = request.RideID;
            RequestDate = request.RequestDate;
            StartLocation = request.StartLocation;
            EndLocation = request.EndLocation;
            IsAccepted = request.IsAccepted;
            DriverName = request.DriverName;
            RiderName = request.RiderName;
            StartDate = request.Date;
            PhotoUrl = request.PhotoUrl;
            PhoneNumber = request.PhoneNumber;
            PickUpDateTime = request.PickupDateTime;
            TripTime = request.TripTime;


            _databaseHelper = databaseHelper;
            _requestsViewModel = requestsViewModel;
            RequestRides = new ObservableCollection<RideRequest>();

            AcceptRequestCommand = new Command(async () => await AcceptRequestAsync());
            DenyRequestCommand = new Command(async () => await DenyRequestAsync());

        }


        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");

        private async Task AcceptRequestAsync()
        {
            await UpdateRequestStatusAsync(true);


            _requestsViewModel.RemoveRequest(this);


        }

        private async Task DenyRequestAsync()
        {
            await UpdateRequestStatusAsync(false);
            //  await _databaseHelper.DeleteRideRequestAsync(RequestId);

            _requestsViewModel.RemoveRequest(this);


        }

        private async Task UpdateRequestStatusAsync(bool isAccepted)
        {
            string RequestID = RequestId;
          //  string RequestID = Preferences.Get("RequestID", string.Empty);

            //    await Application.Current.MainPage.DisplayAlert("Request Updated", RequestId, "OK");

            await firebaseClient
                 .Child("requestRide")
                 .Child(RequestID)
                 .PatchAsync(new { IsAccepted = isAccepted });
            IsAccepted = isAccepted;

            await firebaseClient
            .Child("requestRide")
            .Child(RequestID)
            .DeleteAsync();

            var status = isAccepted ? "accepted" : "denied";
            await Application.Current.MainPage.DisplayAlert("Request Updated", $"The request has been {status}.", "OK");

            if (isAccepted)
            {
                var acceptedTrip = new BookedRide
                {
                    RideID = RideId,
                    RiderID = RiderId,
                    DriverID = DriverId,
                    DriverName = DriverName,
                    RiderName = RiderName,
                    StartLocation = StartLocation,
                    EndLocation = EndLocation,
                    PickupDateTime = PickUpDateTime,
                    Number_of_seats = Number_of_seats,
                    PricePerRide = PricePerRide,
                    PhotoUrl = PhotoUrl,
                    
                };
                await _databaseHelper.DeleteRideRequestAsync(RequestID);

                string tripId = await _databaseHelper.AddAcceptedTripAsync(acceptedTrip);
                acceptedTrip.TripID = tripId;

             //   await _databaseHelper.DeleteRideRequestAsync(RequestId);

                /*
                Device.BeginInvokeOnMainThread(() =>
                {
                    int requestIndex = RequestRides.IndexOf(RequestRides.FirstOrDefault(r => r.RequestID == RequestId));
                    if (requestIndex >= 0)
                    {
                        RequestRides.RemoveAt(requestIndex);
                    }
                });
                */



            }

        }
    }
}

