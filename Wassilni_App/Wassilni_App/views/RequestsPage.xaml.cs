using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestsPage : ContentPage
    {
        private readonly RequestsViewModel _requestsViewModel;

        public ObservableCollection<Ride> RideRequests { get; set; }



        public RequestsPage()
        {
            InitializeComponent();
            _requestsViewModel = new RequestsViewModel();
            BindingContext = _requestsViewModel;
            RequestsCollectionView.ItemsSource = _requestsViewModel.RideRequests;

            MessagingCenter.Subscribe<RequestsViewModel, bool>(this, "CollectionViewEmpty", (sender, isEmpty) =>
            {
                RequestsCollectionView.IsVisible = !isEmpty;
                NoRequestsLabel.IsVisible = isEmpty;
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _requestsViewModel.LoadRideRequests();
        }
    }

}
