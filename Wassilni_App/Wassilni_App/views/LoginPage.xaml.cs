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
using Android.App;
using System.ComponentModel;

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
        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {

                IsLogedIn = true;
                App.Current.MainPage = new NavigationPage(new TabbedBottom());

            }
            else
            {
                DisplayAlert("Message", message, "Ok");
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