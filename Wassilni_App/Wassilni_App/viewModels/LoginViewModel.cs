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

                App.Current.MainPage = new NavigationPage(new TabbedBottom());
            }
            else
            {
                IsLogedIn = false;
                await Application.Current.MainPage.DisplayAlert("Error", message, "Ok");
            }

        }


        /*private async Task SaveUserToDatabase(GoogleUser user)
        {
            FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
            var firebaseObject = await firebaseClient.Child("User").PostAsync(user);
            string firebaseKey = firebaseObject.Key;
            user.FirebaseKey = firebaseKey;
            await firebaseClient.Child("User").Child(firebaseKey).PutAsync(user);

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));

                var authResult = await authProvider.CreateUserWithEmailAndPasswordAsync(user.Email, "WWW123456");

                var newUser = new
                {
                    Name = user.FullName,
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl,
                };

                await firebaseClient.Child("User").Child(authResult.User.LocalId).PutAsync(newUser);
            }
            catch (FirebaseAuthException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Message", ex.Message, "Ok");
            }
        }*/
        
        private async Task SaveUserToDatabase(GoogleUser user)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));
            //var firebaseUser = await authProvider.GetUserAsync(user.FirebaseKey);
            //retrieve the user object from the real-time database
            Console.WriteLine(user.Email);

            //var user = await firebaseClient.Child("User").Child(UserId).OnceSingleAsync<Models.User>();

           // var firebaseObject = await firebaseClient.Child("User").Child(user.).OnceSingleAsync<Models.User>();

            //var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));
           // var firebaseObject = await authProvider.GetUserAsync(user.Email);
            var userSnapshot = await firebaseClient.Child("User").OnceAsync<Models.User>();
            // Retrieve all users

            // var users = new List<Models.User>();
            //object userInfo;
            var user_is_exist = false;
            foreach (var userObject in userSnapshot)
            {
                var userInfo = userObject.Object;
                if(userInfo.Email == user.Email)
                {
                    Console.WriteLine("user is exist in database");
                    user_is_exist = true;
                    //userInfo = userObject.Object;
                    break;
                }
                //Console.WriteLine(userInfo.Email);
                //Console.WriteLine(userInfo.FullName);
                //users.Add(userInfo);
            }



            // get the GoogleUser object from the FirebaseObject
            //GoogleUser firebaseUser = firebaseObject.Object;

            if (user_is_exist)
            {
                Console.WriteLine("sign_in");
                Console.WriteLine(user.Email);
                //var authResult = await authProvider.SignInWithEmailAndPasswordAsync(user.Email, "WWW123456");
                var firebaseObject = await firebaseClient.Child("User").PostAsync(user);
                string userId = firebaseObject.Key;
                user.userId = userId;
                Console.WriteLine("user.userId = " + user.userId);

                id = user.userId;
                Console.WriteLine("Id = " + id);

                Preferences.Set("userId", id);
                if (id != null)
                {
                    await authProvider.SignInWithEmailAndPasswordAsync(user.Email, "WWW123456");
                    await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());
                }

                Console.WriteLine("after sign_in");
            }
            else
            {
                Console.WriteLine(" in the else ");
                var authResult = await authProvider.CreateUserWithEmailAndPasswordAsync(user.Email, "WWW123456");

                //var firebaseObject = await firebaseClient.Child("User").PostAsync(user);
                //string userId = firebaseObject.Key;
                //user.userId = userId;
                await firebaseClient.Child("User").Child(authResult.User.LocalId).PutAsync(user);
                await authProvider.SignInWithEmailAndPasswordAsync(user.Email, "WWW123456");



            }


            /*
            if (firebaseObject != null)
           {
               //try
               //{
                    Console.WriteLine("inside if    = " + firebaseObject.Email);

                    var authResult = await authProvider.SignInWithEmailAndPasswordAsync(user.Email, "WWW123456");

                    Console.WriteLine("authResult   =  "  + authResult);
                    await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());

                    // var userProfile = await authProvider.GetUserAsync(authResult.FirebaseToken);
                    // Console.WriteLine("authResult   =  " + authResult);

                    // if (userProfile.IsEmailVerified)
                    //{
                    //    // User is logged in, navigate to next page
                    //    await Application.Current.MainPage.Navigation.PushAsync(new TabbedBottom());
                    //    EmailErrorMessage = "";
                    //}
               // }
               // catch (FirebaseAuthException ex)
               //{
               //    // Invalid ID token error
               //    await Application.Current.MainPage.DisplayAlert("Message", ex.Message, "Ok");
               //}
           }
           else
           {
               try
               {
                   // Create the new user in the Firebase authentication database
                   var authResult = await authProvider.CreateUserWithEmailAndPasswordAsync(user.Email, "WWW123456");

                   // Save the Firebase user ID to the local user object
                   var newUser = new GoogleUser
                   {
                       FullName = user.FullName,
                       Email = user.Email,
                       PhotoUrl = user.PhotoUrl,
                       UserId = user.UserId,
                       FirebaseKey = authResult.User.LocalId
                   };

                   // Save the user info to the real-time database
                   FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
                   await firebaseClient.Child("User").Child(authResult.User.LocalId).PutAsync(newUser);

                   // User is logged in, navigate to next page
                   App.Current.MainPage = new NavigationPage(new TabbedBottom());
               }
               catch (FirebaseAuthException ex)
               {
                   // Firebase authentication error
                   await Application.Current.MainPage.DisplayAlert("Message", ex.Message, "Ok");
               }
           }*/
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