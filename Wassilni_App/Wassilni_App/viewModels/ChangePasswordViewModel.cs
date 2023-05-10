using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Java.Security;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.views;
using Wassilni_App.views.Settings;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Wassilni_App.viewModels
{
    internal class ChangePasswordViewModel : BaseViewModel
    {
        // Initialize the FirebaseAuthProvider with your Firebase app's API key
        private FirebaseAuthProvider _authProvider;
        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        private string Email;

        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set { SetProperty(ref _oldPassword, value); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { SetProperty(ref _newPassword, value); }
        }
        private string _confirmeNewPassword;
        public string ConfirmNewPassword
        {
            get { return _confirmeNewPassword; }
            set { SetProperty(ref _confirmeNewPassword, value); }
        }
        public Command ChangePasswordCommand { get; }

        public ChangePasswordViewModel()
        {
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0"));

            ChangePasswordCommand = new Command(async () => await ExecuteChangePasswordCommand());
            string userId = Preferences.Get("userId", string.Empty);
            GetUser(userId);
        }
        public async Task GetUser(String UserId)
        {
            var user = await firebaseClient.Child("User").Child(UserId).OnceSingleAsync<Models.User>();
            if (user != null)
            {
                Email = user.Email;
            }
        }
        private string _oldPasswordErrorMessage;
        public string OldPasswordErrorMessage
        {
            get { return _oldPasswordErrorMessage; }
            set { SetProperty(ref _oldPasswordErrorMessage, value); }
        }
        private string _newPasswordErrorMessage;
        public string NewPasswordErrorMessage
        {
            get { return _newPasswordErrorMessage; }
            set { SetProperty(ref _newPasswordErrorMessage, value); }
        }
        private string _confirmeNewPasswordErrorMessage;
        public string ConfirmeNewPasswordErrorMessage
        {
            get { return _confirmeNewPasswordErrorMessage; }
            set { SetProperty(ref _confirmeNewPasswordErrorMessage, value); }
        }
        private async Task ExecuteChangePasswordCommand()
        {
            ClearErrorMassege();
            try
            {
                if (string.IsNullOrEmpty(OldPassword))
                {
                    OldPasswordErrorMessage = "Please enter your old password";
                    return;
                }

                if (string.IsNullOrEmpty(NewPassword))
                {
                    NewPasswordErrorMessage = "Please enter your new password";
                    return;
                }
                if (string.IsNullOrEmpty(ConfirmNewPassword))
                {
                    ConfirmeNewPasswordErrorMessage = "Please rewrite your new password";
                    return;
                }
                if (NewPassword != ConfirmNewPassword)
                {
                    ConfirmeNewPasswordErrorMessage = "Passwords don't match";
                    return;
                }
                FirebaseAuthLink auth = null;
                try
                {
                    auth = await _authProvider.SignInWithEmailAndPasswordAsync(Email, OldPassword);
                }
                catch (FirebaseAuthException ex)
                {
                    // Display an error message if the old password is incorrect
                    OldPasswordErrorMessage = "Incorrect old password";
                    return;
                }
                await _authProvider.ChangeUserPassword(auth.FirebaseToken, NewPassword);

                // Display a success message to the user
                await PopupNavigation.Instance.PushAsync(new ChangePasswordPopUp());
                ClearEntries();
            }
            catch (FirebaseAuthException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Reason.ToString(), "OK");
            }
        }
        public void ClearEntries()
        {
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
        }
        public void ClearErrorMassege()
        {
            OldPasswordErrorMessage = string.Empty;
            NewPasswordErrorMessage = string.Empty;
            ConfirmeNewPasswordErrorMessage = string.Empty;
        }
    }
}
