using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Xamarin.Forms;
using Wassilni_App.viewModels;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using Xamarin.Essentials;
using System.Diagnostics;
using Wassilni_App.Services;
using Org.Xmlpull.V1.Sax2;

namespace Wassilni_App.viewModels
{

    public class RequestsViewModel : BaseViewModel
    {

        private readonly DatabaseHelper _databaseHelper;

        public ObservableCollection<RequestViewModel> RideRequests { get; set; }

        /** 
         public RequestsViewModel()
         {
             _databaseHelper = new DatabaseHelper("https://wassilni-app-default-rtdb.firebaseio.com/");

             LoadRideRequests();
         }
        */
        /*
           public async Task LoadRideRequests()
           {
               var rideRequests = await _databaseHelper.GetRideRequestsAsync();
               RideRequests = new ObservableCollection<RequestViewModel>(rideRequests.Select(r => new RequestViewModel(r)));
           }
        */
        public RequestsViewModel()
        {
            RideRequests = new ObservableCollection<RequestViewModel>();
            _databaseHelper = new DatabaseHelper("https://wassilni-app-default-rtdb.firebaseio.com/"); // Replace with your actual DatabaseHelper constructor
        }

        public async Task LoadRideRequests()
        {
            var requests = await _databaseHelper.GetRideRequestsAsync(); // Replace with your actual method to fetch ride requests from the database

            RideRequests.Clear();
            foreach (var request in requests)
            {
                RideRequests.Add(new RequestViewModel(request, this, _databaseHelper));
            }
        }
        public void RemoveRequest(RequestViewModel request)
        {
            RideRequests.Remove(request);
        }
    }
}

