using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        async private void GoToForgetPassPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgetPasswordPage());
        }

        async private void GoToTheHomePage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new TabbedBottom()));
        }

        async private void GoToTheSignupPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }





    }
}