using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Auth;

namespace Wassilni_App.views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        private FirebaseAuthProvider _authProvider;
        public ChangePasswordPage()
        {
            InitializeComponent();
            this.BindingContext = new ChangePasswordViewModel();
            // Initialize the FirebaseAuthProvider with your Firebase app's API key
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0"));
        }
        
        private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
        {

            try
            {
                // Retrieve the user's email and passwords from the Entry controls
                string email = EmailEntry.Text;
                string oldPassword = OldPasswordEntry.Text;
                string newPassword = NewPasswordEntry.Text;
                if (string.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Change Password", "please Type email", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(oldPassword))
                {
                    await DisplayAlert("Change Password", "please Type oldpasswored", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(newPassword))
                {
                    await DisplayAlert("Change Password", "please Type newpasswored", "OK");
                    return;
                }
                // Sign in the user with their email and old password
                FirebaseAuthLink auth = await _authProvider.SignInWithEmailAndPasswordAsync(email, oldPassword);

                // Change the user's password using the ChangeUserPassword method
                await _authProvider.ChangeUserPassword(auth.FirebaseToken, newPassword);

                // Display a success message to the user
                await DisplayAlert("Password Changed", "Your password has been changed successfully.", "OK");
            }
            catch (FirebaseAuthException ex)
            {
                // Display an error message to the user if the password change fails
                await DisplayAlert("Error", ex.Reason.ToString(), "OK");
            }
        }
        private async void GoToSettingsPage(object sender, EventArgs e)
        {
          await Navigation.PushAsync(new SettingsPage());
        }
    }
}