using Firebase.Auth;
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
        //public DateTime PickUpDateTime { get; set; }
        public String TripDate { get; set; }
        public String TripTime { get; set; }
        public DateTime StartDate { get; set; }
        public string PhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public decimal PricePerRide { get; set; }
        public string Gender { get; set; }
        public List<Rider> Riders { get; set; }

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
            TripDate = request.PickupDateTime.Date.ToString("dd-MM-yyyy");
            TripTime = request.PickupDateTime.ToString("hh:mm:ss tt");
            PhotoUrl = request.PhotoUrl;
            Gender = request.SelectedGender;
            Number_of_seats = request.Number_of_Seats;
            Riders=request.Riders;


            _databaseHelper = databaseHelper;
            _requestsViewModel = requestsViewModel;
            RequestRides = new ObservableCollection<RideRequest>();

            AcceptRequestCommand = new Command(async () => await AcceptRequestAsync());
            DenyRequestCommand = new Command(async () => await DenyRequestAsync());

        }

        string userId = Preferences.Get("userId", string.Empty);

        private readonly FirebaseClient firebaseClient = new FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");

        private async Task AcceptRequestAsync()
        {
            string RideID = RideId;
        
            string userName = await FetchUserName(userId);
            await UpdateRequestStatusAsync(true);

          
            Ride bookedRide = await _databaseHelper.GetBookedRideByRideIdAsync(RideID);
         
            if (bookedRide == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Not Found", "ok");
            }
           

            bookedRide.Riders.Add(new Rider
            {
              RiderID=RiderId,
              RiderName=RiderName,
              Number_of_seats=Number_of_seats,
              Gender=Gender,
              RiderPhotoUrl=PhotoUrl,
              
              
            });
            await firebaseClient
           .Child("Ride")
            .Child(RideID)
            .PutAsync(bookedRide);


          await UpdateAvailableSeatsAsync(Number_of_seats);

         
            await _databaseHelper.DeleteRideRequestAsync(RequestId);


            
           _requestsViewModel.RemoveRequest(this);
            _requestsViewModel.RemoveRequest(this);
            string userFcmToken = await FetchDriverFcmToken(RiderId);
            var pushNotificationHelper = new PushNotificationHelper();
            await pushNotificationHelper.SendNotificationAsync(
                "Ride Request Accepted",
                $"Your ride request has been accepted by {bookedRide.DriverName}. Please check your upcoming rides.",
                userFcmToken, RiderId, PhotoUrl);

        }

        private async Task DenyRequestAsync()
        {
            await UpdateRequestStatusAsync(false);

            _requestsViewModel.RemoveRequest(this);
            string userFcmToken = await FetchDriverFcmToken(RiderId);
            var pushNotificationHelper = new PushNotificationHelper();
            await pushNotificationHelper.SendNotificationAsync(
                "Ride Request Denayed",
                $"Your ride request has been Denayed by {DriverName}.",
                userFcmToken,RiderId, PhotoUrl);


        }

        private async Task UpdateRequestStatusAsync(bool isAccepted)
        {
            string RequestID = RequestId;
           

            await firebaseClient
                 .Child("requestRide")
                 .Child(RequestID)
                 .PatchAsync(new { IsAccepted = isAccepted });
            IsAccepted = isAccepted;

            await firebaseClient
            .Child("requestRide")
            .Child(RequestID)
            .DeleteAsync();

          //  var status = isAccepted ? "accepted" : "denied";
          //  await Application.Current.MainPage.DisplayAlert("Request Updated", $"The request has been {status}.", "OK");

        }



        private async Task UpdateAvailableSeatsAsync(int numberOfSeats)
        {
            var ride = await firebaseClient
                .Child("Ride")
                .Child(RideId)
                .OnceSingleAsync<Ride>();

            ride.Number_of_seats -= numberOfSeats;

            await firebaseClient
                .Child("Ride")
                .Child(RideId)
                .PutAsync(ride);
        }
        private async Task<string> FetchDriverFcmToken(string driverId)
        {

            var driverSnapshot = await firebaseClient
                .Child("User")
                .Child(driverId)
                .OnceSingleAsync<Wassilni_App.Models.User>();

            return driverSnapshot.FCMToken;
        }
        private async Task<string> FetchUserName(string userId)
        {

            var userSnapshot = await firebaseClient
                .Child("User")
                .Child(userId)
                .OnceSingleAsync<Wassilni_App.Models.User>();



            return $"{userSnapshot.FirstName} {userSnapshot.LastName}";
        }
    }

}


