using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using System.Drawing;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;

namespace Wassilni_App.viewModels
{
    public class ProfileViewModel : BaseViewModel
    {


        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");


        public String _Name;
        public String Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
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
        private String _PersonalPhoto;
        public String PersonalPhoto
        {
            get { return _PersonalPhoto; }
            set { SetProperty(ref _PersonalPhoto, value); }
        }

        public ProfileViewModel()
        {
            string userId = Preferences.Get("userId", string.Empty);
            GetUser(userId);
        }

        public async Task GetUser(String UserId)
        {
            var user = await firebaseClient.Child("User").Child(UserId).OnceSingleAsync<Models.User>();
            //var googleUser = await firebaseClient.Child("User").Child(UserId).OnceSingleAsync<GoogleUser>();

            if (user != null)
            {
                Email = user.Email;
                PersonalPhoto = user.PhotoUrl;
                Name = user.FirstName + " " + user.LastName;
                PhoneNumber = user.PhoneNumber;
                Gender = await firebaseClient.Child("User").Child(UserId).Child("Gender").OnceSingleAsync<String>();
                Age = (DateTime.Now - DateTime.Parse(user.Birthdate)).Days / 365;
            }
         
            else
            {
                Console.WriteLine("*******************************************");
            }
            
        }
    }
}
