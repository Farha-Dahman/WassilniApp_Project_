using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class RideViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");

        public ICommand RequestRideCommand { get; set; }
        public ICommand OnItemTapped { get; set; }


        public string RideId { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public decimal PricePerRide { get; set; }
        public int Number_of_seats { get; set; }
        public string RiderName { get; set; }
        public string DriverName { get; set; }
        public string DriverId { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Date { get; set; }

        public String TripDate { get; set; }
        public List<Rider> Riders { get; set; }

        public TimeSpan TripTime { get; set; }

        public RideViewModel(Ride ride)
        {
       
            DriverName = ride.DriverName;
            StartLocation = ride.StartLocation;
            EndLocation = ride.EndLocation;
            PricePerRide = ride.PricePerRide;
            Number_of_seats = ride.Number_of_seats;
            PickupDateTime = ride.PickupDateTime;
            DriverId = ride.DriverID;
            PhoneNumber = ride.PhoneNumber;
            RideId = ride.RideID;
            PhotoUrl = ride.PhotoUrl;
            TripTime = ride.TripTime;
            TripDate = ride.Date.Date.ToString("yyyy-MM-dd");
            Riders = ride.Riders;

            RequestRideCommand = new Command(RequestRide);
            OnItemTapped = new Command<string>(OnFrameClicked);


        }

        private async void OnFrameClicked(string data)
        {
            // Navigate to the second page with the passed data
            //Console.WriteLine("***** data: ");
            //Console.WriteLine(data);'

            await Application.Current.MainPage.Navigation.PushAsync(new TripDetailsPage(data));
        }

        private async void RequestRide()
        {

            string userId = Preferences.Get("userId", string.Empty);
            //  string DriverId = Preferences.Get("DriverId", string.Empty);
            string rideId = RideId;


         



            try
            {
                  bool hasRequestedRide = await CheckIfUserHasRequestedRide(rideId, userId);

                if (!hasRequestedRide)
                {

                    string userName = await FetchUserName(userId);



                    var newRideRequest = new RideRequest
                    {
                        PhotoUrl = PhotoUrl,
                        RiderID = userId,
                        DriverID = DriverId,
                        RequestDate = DateTime.Now,
                        StartLocation = StartLocation,
                        EndLocation = EndLocation,
                        PickupDateTime = PickupDateTime,
                        Date = Date,
                        TripTime = TripTime,
                        IsAccepted = false,
                        DriverName = DriverName,
                        RiderName = userName,
                        RideID = RideId,
                        PhoneNumber = PhoneNumber,
                        TripDate=TripDate,
                        Number_of_Seats=Number_of_seats,
                    };
                    var newRideRequestResponse = await firebaseClient
                   .Child("requestRide")
                   .PostAsync(newRideRequest);

                    string requestID = newRideRequestResponse.Key;

                    newRideRequest.RequestID = requestID;
                    //  await Application.Current.MainPage.DisplayAlert("Request Sent", requestId, "OK");
                    await firebaseClient
                    .Child("requestRide")
                    .Child(requestID)
                    .PatchAsync(new { RequestID = requestID });

                    Preferences.Set("RequestID", newRideRequest.RequestID);


                    await PopupNavigation.Instance.PushAsync(new PopUpSuccessRequest());

                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new PopUpDeniedRequest());

                }
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Already Requested", ex.Message, "OK");

            }


        }


        private async Task<bool> CheckIfUserHasRequestedRide(string rideId, string userId)
        {
            var requestExists = await firebaseClient
                .Child("requestRide")
                .OrderBy("RideID")
                .EqualTo(rideId)
                .OnceAsync<RideRequest>();

            return requestExists.Any(r => r.Object.RiderID == userId);
        }
        private async Task<string> FetchUserName(string userId)
        {
            var firebaseClient = new FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");

            var userSnapshot = await firebaseClient
                .Child("User")
                .Child(userId)
                .OnceSingleAsync<Wassilni_App.Models.User>();



            return $"{userSnapshot.FirstName} {userSnapshot.LastName}";
        }
    }



} 

