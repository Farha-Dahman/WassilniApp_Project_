using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Wassilni_App.viewModels;
using Firebase.Auth;
using Xamarin.Essentials;
using Android.App;
using Wassilni_App.Services;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NotificationsPage : ContentPage
    {
        public DatabaseHelper _dataBaseHelper;

        NotificationViewModel viewModel;

        public NotificationsPage()
        {
          //  BindingContext = new NotificationViewModel();

            InitializeComponent();
            _dataBaseHelper = new DatabaseHelper("https://wassilni-app-default-rtdb.firebaseio.com/");

        }

        private async Task LoadNotificationsAsync()
        {
            var notifications = await _dataBaseHelper.GetUserNotificationsAsync();
            foreach (var notification in notifications)
            {
                Debug.WriteLine($"Title: {notification.Title}, Message: {notification.Message}");
            }
            NotificationsCollectionView.ItemsSource = notifications;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadNotificationsAsync();
        }
    }
}