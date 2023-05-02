using Wassilni_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wassilni_App.viewModels;
using Wassilni_App.views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            _googleManager = DependencyService.Get<IGoogleManager>();
            this.BindingContext = new LoginViewModel();
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
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
                    Name = googleUser.Name,
                    Email = googleUser.Email,
                    PhotoUrl = googleUser.Picture.AbsoluteUri,
                    UserId = googleUser.UserId,
                };
                await SaveUserToDatabase(user);

                App.Current.MainPage = new NavigationPage(new TabbedBottom());

            }
            else
            {
                DisplayAlert("Message", message, "Ok");
            }
        }
        private async Task SaveUserToDatabase(GoogleUser user)
        {
            FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
            var firebaseObject = await firebaseClient.Child("User").PostAsync(user);
            string firebaseKey = firebaseObject.Key;
            user.FirebaseKey = firebaseKey;
            await firebaseClient.Child("User").Child(firebaseKey).PutAsync(user);

            FirebaseApp app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson("YOUR_SERVICE_ACCOUNT_KEY_JSON"),
                ProjectId = "YOUR_PROJECT_ID"
            });

            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            try
            {
                var result = await auth.CreateUserWithEmailAndPasswordAsync(user.Email, "RANDOM_PASSWORD");
                // User created successfully in Firebase Authentication
            }
            catch (FirebaseAuthException ex)
            {
                // Handle any errors here
            }
        }

        private void GoogleLogout()
        {
            _googleManager.Logout();
            IsLogedIn = false;
        }
        private void btnLogout_Clicked(object sender, EventArgs e)
        {
            _googleManager.Logout();

        }

        async private void GoToTheHomePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new TabbedBottom()));

        }

        async private void GoToTheSignupPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SignupPage()));

        }
        async private void ForgetPass_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new ForgetPasswordPage()));

        }


    }

}