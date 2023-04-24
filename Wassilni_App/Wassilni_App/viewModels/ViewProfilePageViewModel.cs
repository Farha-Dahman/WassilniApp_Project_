using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace Wassilni_App.viewModels
{
    public class ViewProfilePageViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient;

        public String _FirstName;
        public String FirstName
        {
            get { return _FirstName; }
            set { SetProperty(ref _FirstName, value); }
        }

        public String _LastName;
        public String LastName
        {
            get { return _LastName; }
            set { SetProperty(ref _LastName, value); }
        }

        private String _PhoneNumber;
        public String PhoneNumber
        {
            get { return _PhoneNumber; }
            set { SetProperty(ref _PhoneNumber, value); }
        }
        private String _Email;
        public String Email
        {
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
        }
        private String _Gender;
        public String Gender
        {
            get { return _Gender; }
            set { SetProperty(ref _Gender, value); }
        }
        private int _Age;
        public int Age
        {
            get { return _Age; }
            set { SetProperty(ref _Age, value); }
        }
        private String _PhotoUrl;
        public String PhotoUrl
        {
            get { return _PhotoUrl; }
            set { SetProperty(ref _PhotoUrl, value); }
        }

        public ViewProfilePageViewModel(String email)
        {
            firebaseClient = new FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
            LoadUser(email);
        }
        private async void LoadUser(string email)
        {
            var users = await firebaseClient.Child("User").OnceAsync<Models.User>();
            var user = users.Select(u => u.Object).FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.Email;
                PhoneNumber = user.PhoneNumber;
                var userQuery = await firebaseClient.Child("User")
                                .OrderBy("Email")
                                .EqualTo(email)
                                .OnceAsync<Models.User>();
                var userNode = userQuery.First().Key;
                Gender = await firebaseClient.Child("User").Child(userNode).Child("Gender").OnceSingleAsync<string>();
                Age = (DateTime.Now - DateTime.Parse(user.Birthdate)).Days / 365;
                PhotoUrl = user.PhotoUrl;
            }
        }
    }
}