using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.Services;
using Wassilni_App.viewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTripsPage : ContentPage
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly string _driverid;
        public ObservableCollection<Ride> BookedRides { get; set; }

        public MyTripsPage()
        {
            InitializeComponent();
            this.BindingContext = new MyTripsViewModel();
            _databaseHelper = new DatabaseHelper("https://wassilni-app-default-rtdb.firebaseio.com/"); 

            _driverid = Preferences.Get("userId", string.Empty);

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ReloadRides();
        }
        private async Task ReloadRides()
        {
            var rides = await LoadRides();
            var ridesWithRider = await LoadRidesWithRiderInfo();
            var allRides = new List<Ride>(rides.Concat(ridesWithRider));

            PoolsCollectionView.ItemsSource = allRides;
        }

        //async private void GoToPoolDetails(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new NavigationPage(new TripDetailsPage()));



        private async void OnCancelClicked(object sender, EventArgs e)
        {
            string tripId = (string)((Button)sender).CommandParameter;
            string userId = Preferences.Get("userId", string.Empty);

            FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
            var ride = await firebaseClient.Child("Ride").Child(tripId).OnceSingleAsync<Ride>();

            //cancel trip from driver
            if (ride != null && userId == ride.DriverID)
            {
                await firebaseClient.Child("Ride").Child(tripId).DeleteAsync();
                await ReloadRides();
            }

            //cancel trip from rider
            else if (ride != null && userId != ride.DriverID)
            {
                for (int i = 1; ; i++)
                {
                    try
                    {
                        var Rider = await firebaseClient.Child("Ride").Child(tripId).Child("Riders").Child(i.ToString()).OnceSingleAsync<Rider>();
                        if (Rider == null)
                        {
                            Console.WriteLine("No more riders.");
                            break;
                        }
                        if (Rider != null && Rider.RiderID == userId)
                        {
                            //var NumberOfSeatsForRider = Rider.Number_of_seats;
                            var NumberOfSeatsForRider = await firebaseClient.Child("Ride").Child(tripId).Child("Riders").Child(i.ToString()).Child("Number_of_seats").OnceSingleAsync<int>();
                            await firebaseClient.Child("Ride").Child(tripId).Child("Riders").Child(i.ToString()).DeleteAsync();
                            var NewNumberOfSeats = ride.Number_of_seats - NumberOfSeatsForRider;
                            await firebaseClient.Child("Ride").Child(tripId).Child("Number_of_seats").PutAsync(NewNumberOfSeats);
                            await ReloadRides();
                        }
                    } 
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

        }
      
        private async Task<List<Ride>> LoadRidesWithRiderInfo()
        {
            try
            {
                var ridesWithRiderInfo = await _databaseHelper.GetRidesWithRidersByUserIdAsync(_driverid);
                return ridesWithRiderInfo;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"error occurred while loading rides: {ex.Message}", "OK");
                return new List<Ride>();
            }
       }

        private async Task<List<Ride>> LoadRides()
        {
            try
            {
                var rideList = await _databaseHelper.LoadRides(_driverid);
                return rideList;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $" error occurred while loading Driver rides:", ex.Message, "OK");
                return new List<Ride>(); 
            }
        }

    }
}
