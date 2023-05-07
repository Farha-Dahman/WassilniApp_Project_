using Firebase.Database;
using Firebase.Database.Query;
using Java.Sql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Xamarin.Essentials;
using static Android.Icu.Text.CaseMap;
using static Android.Views.WindowInsets;

namespace Wassilni_App.viewModels
{

   


    public class NotificationViewModel : BaseViewModel
    {
        private string _photoUrl;
        private string _title;
        private string _message;
        private Timestamp _timestamp;

        public String PhotoUrl
        {
            get { return _photoUrl; }
            set { SetProperty(ref _photoUrl, value); }
        }
        public string Title
        {

            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public string Message
        {

            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        public Timestamp Timestamp
        {

            get { return _timestamp; }
            set { SetProperty(ref _timestamp, value); }
        }

        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get => _notifications;
          set { SetProperty(ref _notifications, value); }
            
        }

        private FirebaseClient firebaseClient;

        public NotificationViewModel()
        {

            Notifications = new ObservableCollection<Notification>();
            firebaseClient = new FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
            GetUserNotificationsAsync();
        }


        public async Task<List<Notification>> GetUserNotificationsAsync()
        {
            string userId = Preferences.Get("userId", string.Empty);
            try
            {
                var notifications = await firebaseClient
                    .Child("UserNotifications")
                    .Child(userId)
                    .OnceAsync<Notification>();

                var notif = notifications.ToList();

                var not = notif.Select(n => new Notification
                {
                    PhotoUrl = n.Object.PhotoUrl,
                    Title = n.Object.Title,
                    Message = n.Object.Message,
                }).ToList();

                Notifications = new ObservableCollection<Notification>(not);
                return not;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving notifications: {ex.Message}");
                return new List<Notification>();
            }
        }

        public async void LoadNotifications()
        {
            string userid = Preferences.Get("userId", string.Empty);

            var userId = userid;
            var notifications = await firebaseClient
                .Child("UserNotifications")
                .Child(userId)
                .OnceAsync<Notification>();

            Notifications.Clear();

            foreach (var notification in notifications)
            {
                var newNotification = new Notification
                {
                    PhotoUrl = notification.Object.PhotoUrl,
                    Title = notification.Object.Title,
                    Message = notification.Object.Message,
                    Timestamp = notification.Object.Timestamp
                };

                Notifications.Add(newNotification);
            }
        }

    }
}
