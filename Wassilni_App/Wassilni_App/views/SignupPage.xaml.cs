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
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
            this.BindingContext = new SignupViewModel();
        }
        async private void GoToVerificationPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new VerificationCodePage()));
        }
        async private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));

        }
        async private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
        }
       
        private bool isPasswordVisible;

        private void OnEyeIconTapped(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            PasswordEntry.IsPassword = !isPasswordVisible;

            // Update the eye icon
            if (isPasswordVisible)
            {
                EyeIcon.Text = "\xf070"; // Replace with the closed eye icon resource
            }
            else
            {
                EyeIcon.Text = "\xf06e"; // Replace with the open eye icon resource
            }
        }
    }
}