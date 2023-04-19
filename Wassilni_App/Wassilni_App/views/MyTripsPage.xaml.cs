using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Wassilni_App.Services;
using Wassilni_App.viewModels;
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
            _databaseHelper = new DatabaseHelper("https://wassilni-app-default-rtdb.firebaseio.com/"); // Initialize the _databaseHelper field

            _driverid = Xamarin.Essentials.Preferences.Get("userId", string.Empty);

        }

        async private void GoToPoolDetails(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new TripDetailsPage()));

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var rides = await LoadRides();
            //   var ridesWithRiderInfo = await LoadRidesWithRiderInfo();

            // Combine the lists
              var allRides = new List<Ride>(rides);

            // Set the combined list as the ItemsSource for the RidesCollectionView
            PoolsCollectionView.ItemsSource = allRides;
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
