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
using Google.Apis.Auth.OAuth2;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;

namespace Wassilni_App.viewModels
{
    public class LoginViewModel : BaseViewModel
    {

        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        FirebaseAuthProvider authProvider;
        string webAPIkey = "AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0";

        private readonly IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }


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

        public ICommand LoginWithGoogle { get; set; }



        public LoginViewModel()
        {
            SignInCommand = new Command(async () => await ExecuteSignInCommand());
            _googleManager = DependencyService.Get<IGoogleManager>();
            LoginWithGoogle = new Command(async () => await ExecuteLoginWithGoogleCommand());
        }

        private async Task ExecuteLoginWithGoogleCommand()
        {
            _googleManager.Login(OnLoginComplete);
        }
        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                IsLogedIn = true;
                var user = new GoogleUser
                {
                    FullName = googleUser.FullName,
                    Email = googleUser.Email,
                    PhotoUrl = googleUser.Picture.AbsoluteUri,
                };

                await SaveUserToDatabase(user);
                await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());

            }
            else
            {
                IsLogedIn = false;
                await Application.Current.MainPage.DisplayAlert("Error", message, "Ok");
            }

        }

        private async Task SaveUserToDatabase(GoogleUser user)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));
            var userSnapshot = await firebaseClient.Child("User").OnceAsync<Models.User>();

            var userIsExist = false;
            foreach (var userObject in userSnapshot)
            {
                var userInfo = userObject.Object;
                if (userInfo.Email == user.Email)
                {
                    userIsExist = true;
                    break;
                }
            }
            if (userIsExist)
            {
                var query = await firebaseClient.Child("User").OrderBy("Email").EqualTo(user.Email).OnceAsync<Models.User>();
                string userId = null;
                foreach (var googleUser in query)
                {
                    userId = googleUser.Key;
                    break;
                }
                id = userId;
                Preferences.Set("userId", id);
                if (id != null)
                {
                    await authProvider.SignInWithEmailAndPasswordAsync(user.Email, "WWW123456");
                    await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());
                }
            }
            else
            {
                var authResult = await authProvider.CreateUserWithEmailAndPasswordAsync(user.Email, "WWW123456");

                var firebaseObject = await firebaseClient.Child("User").PostAsync(user);
                string userId = firebaseObject.Key;
                user.userId = userId;

                Preferences.Set("userId", user.userId);
                if (user.userId != null)
                {
                    await authProvider.SignInWithEmailAndPasswordAsync(user.Email, "WWW123456");
                    await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());
                }
            }
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
                        Console.WriteLine($"id: {id}");
                        Preferences.Set("userId", id);
                        if (id != null)
                        {
                            var authResult = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);

                           
                            var userProfile = await authProvider.GetUserAsync(authResult.FirebaseToken);

                            if (userProfile.IsEmailVerified)
                            {
                                await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());
                                EmailErrorMessage = "";
                            }
                            else
                            {
                                EmailErrorMessage = "Please verify your email before signing in.";
                            }
                        }
                    }
                }
                else
                {
                    // Email does not exist in Firebase
                    EmailErrorMessage = "Email Does Not Exist";
                    Console.WriteLine("EmailErrorMessage");
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