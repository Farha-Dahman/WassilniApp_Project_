using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.views;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{

    public class ForgetPasswordViewModel : BaseViewModel  
    {

        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        FirebaseAuthProvider authProvider;
        string webAPIkey = "AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0";



        private string _email;

        private bool _isBusy;
        private string _emailErrorMessage;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
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
        public ICommand ForgetPasswordCommand { get; set; }

        public ForgetPasswordViewModel()
        {
            ForgetPasswordCommand = new Command(async () => await ExecuteForgetPasswordCommand());
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }
        private async Task ExecuteForgetPasswordCommand()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));


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
                var emailExistence = await firebaseClient
                  .Child("User")
                 .OrderBy("Email")
                 .EqualTo(Email)
                 .OnceAsync<object>();
                if (emailExistence.Count > 0)
                {
                    // Email already exists in Firebase
                    await authProvider.SendPasswordResetEmailAsync(Email);
                    await PopupNavigation.Instance.PushAsync(new ResetPasswordPopUp());

                    EmailErrorMessage = "";
                }
                else if (string.IsNullOrEmpty(Email) || !IsValidEmail(Email))
                {
                    EmailErrorMessage = "Please enter a valid email address ";
                }
                else
                {
                    // Email does not exist in Firebase
                    EmailErrorMessage = "Email Does Not Exist";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally { IsBusy = false; }
        }
    }
}