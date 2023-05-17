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
        *
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
            _databaseHelper = new DatabaseHelper("https://wassilni-app-default-rtdb.firebaseio.com/"); 
        }

        public async Task LoadRideRequests()
        {
            var requests = await _databaseHelper.GetRideRequestsAsync();

            RideRequests.Clear();
            foreach (var request in requests)
            {
                RideRequests.Add(new RequestViewModel(request, this, _databaseHelper));
            }
            CheckIfCollectionViewIsEmpty();
        }
        public void RemoveRequest(RequestViewModel request)
        {
            RideRequests.Remove(request);
        }
        private void CheckIfCollectionViewIsEmpty()
        {
            if (RideRequests == null || !RideRequests.Any())
            {
                // Show the empty image
                MessagingCenter.Send(this, "CollectionViewEmpty", true);
            }
            else
            {
                // Show the collection view
                MessagingCenter.Send(this, "CollectionViewEmpty", false);
            }
        }

    }
}

