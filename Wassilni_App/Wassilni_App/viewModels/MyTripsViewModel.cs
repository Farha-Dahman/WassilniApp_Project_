using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Wassilni_App.Models;
using Wassilni_App.Services;
using Xamarin.Essentials;

namespace Wassilni_App.viewModels
{
    public class MyTripsViewModel
    {
        public ObservableCollection<Ride> BookedRides { get; set; }
        public string RiderName { get; set; }
        public DatabaseHelper _databaseHelper;

        public MyTripsViewModel()
        {
            BookedRides = new ObservableCollection<Ride>();

      //   LoadBookedRides();
        }
/*
        private async void LoadBookedRides()
        {
            string userId = Preferences.Get("userId", string.Empty);
            List<Ride> bookedRides = await _databaseHelper.GetBookedRidesByUserIdAsync(userId);
           
            BookedRides.Clear();
            foreach (var ride in bookedRides)
            {
                BookedRides.Add(ride);
            }
        }
*/
    }

}
