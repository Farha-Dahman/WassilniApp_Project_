using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
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

        public MyTripsPage()
        {
            InitializeComponent();
            this.BindingContext = new MyTripsViewModel();
            _databaseHelper = new DatabaseHelper("https://wassilni-app-default-rtdb.firebaseio.com/"); 

            _driverid = Preferences.Get("userId", string.Empty);

        }

        //async private void GoToPoolDetails(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new NavigationPage(new TripDetailsPage()));
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

           var rides = await LoadRides();
        
              var allRides = new List<Ride>(rides);

            PoolsCollectionView.ItemsSource = allRides;
        }

    
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            string tripId = (string)((Button)sender).CommandParameter;

            string userId = Preferences.Get("userId", string.Empty);
            FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
            var ride = await firebaseClient.Child("Ride").Child(tripId).OnceSingleAsync<Ride>();
            //string DriverId = "BcqwrYUZrfWqvUWst3qYVrWxWKJ3";
            //string USerId = "BcqwrYUZrfWqvUWst3qYVrWxWKJ3";
            if (ride != null && userId == ride.DriverID)
            {
                await firebaseClient.Child("Ride").Child(tripId).DeleteAsync();
            }
            else if(ride != null && userId != ride.DriverID) 
            {
                for(int i =1; i <= ride.Number_of_seats; i++)
                {
                    var riderId = await firebaseClient.Child("Ride").Child(tripId).Child("Riders").Child(i.ToString()).Child("RiderId").OnceSingleAsync<string>();
                    if(riderId == userId)
                    {
                        await firebaseClient.Child("ride").Child(tripId).Child("Riders").Child(i.ToString()).DeleteAsync();

                    }
                }
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
