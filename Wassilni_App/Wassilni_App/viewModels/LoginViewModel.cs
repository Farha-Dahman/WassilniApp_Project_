using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.views;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Resource;

namespace Wassilni_App.viewModels
{
    public class LoginViewModel : BaseViewModel
    {

        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        FirebaseAuthProvider authProvider;
        string webAPIkey = "AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0";

        public string id;

        private string _email;
        private string _password;
        private string _emailErrorMessage;
        private string _passwordErrorMessage;

        private bool _isBusy;
     
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public new bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        public string EmailErrorMessage
        {
            get { return _emailErrorMessage; }
            set { SetProperty(ref _emailErrorMessage, value); }
        }
        public string PasswordErrorMessage
        {
            get { return _passwordErrorMessage; }
            set { SetProperty(ref _passwordErrorMessage, value); }
        }
        public ICommand SignInCommand { get; set; }





        public LoginViewModel()
        {
            SignInCommand = new Command(async () => await ExecuteSignInCommand());
        }
        private async Task ExecuteSignInCommand()
        {


            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    EmailErrorMessage = "Please enter a valid email address.";

                }
                else
                {
                    EmailErrorMessage = "";
                }
                if (string.IsNullOrEmpty(Password))
                {
                    PasswordErrorMessage = "Please enter your password.";

                }
                else
                {
                    PasswordErrorMessage = "";
                }

                authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));
                var emailExistence = await firebaseClient
                .Child("User")
               .OrderBy("Email")
               .EqualTo(Email)
               .OnceAsync<object>();
                if (emailExistence.Count > 0)
                {
                    foreach(var user in emailExistence)
                    {
                        id = user.Key;
                        Preferences.Set("userId", id);
                        if (id != null)
                        {
                            await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                            await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());
                            EmailErrorMessage = "";
                        }
                    }
                }
                else
                {

                    // Email does not exist in Firebase
                    EmailErrorMessage = "Email Does Not Exist";
                }

            }
            catch 
            {
                PasswordErrorMessage = "The Password you provided is wrong";
            }



            finally
            {
                IsBusy = false;
            }
        }
    }
}