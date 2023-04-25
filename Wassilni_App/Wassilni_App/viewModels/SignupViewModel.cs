using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Wassilni_App.views;
using Rg.Plugins.Popup.Services;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wassilni_App.viewModels
{
    public class SignupViewModel : BaseViewModel
    {
        private readonly string[] _genders = { "Male", "Female" };
        public IEnumerable<string> Genders => _genders;

        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        string webAPIkey = "AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0";

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _confirmpass;
        private bool _isBusy;
        private string _selectedGender;
        private string _phoneNumber;
        private string _emailErrorMessage;
        private string _emptyFieldsFirstNameErrorMessage;
        private string _passwordErrorMessage;
        private string _BirthdateErrorMessage;
        private string _gendersErrorMessage;
        private bool _isEmailValid = true;
        private string emptyFieldsErrorMessage;
        private bool showEmptyFieldsError;
        private string emptyFieldsLastNameErrorMessage;
        private string _phoneNumberErrorMessage;

        /*error messages*/



        public string PhoneNumberErrorMessage
        {
            get => _phoneNumberErrorMessage;
            set => SetProperty(ref _phoneNumberErrorMessage, value);
        }
        public string GendersErrorMessage
        {
            get => _gendersErrorMessage;
            set => SetProperty(ref _gendersErrorMessage, value);
        }
        public string BirthdateErrorMessage
        {
            get { return _BirthdateErrorMessage; }
            set { SetProperty(ref _BirthdateErrorMessage, value); }
        }
        public string PasswordErrorMessage
        {
            get { return _passwordErrorMessage; }
            set { SetProperty(ref _passwordErrorMessage, value); }
        }
        public string EmailErrorMessage
        {
            get { return _emailErrorMessage; }
            set { SetProperty(ref _emailErrorMessage, value); }
        }
        public string EmptyFieldsFirstNameErrorMessage
        {
            get { return _emptyFieldsFirstNameErrorMessage; }
            set { SetProperty(ref _emptyFieldsFirstNameErrorMessage, value); }
        }
        public string EmptyFieldsErrorMessage
        {
            get { return emptyFieldsErrorMessage; }
            set { SetProperty(ref emptyFieldsErrorMessage, value); }
        }
        public string EmptyFieldsLastNameErrorMessage
        {
            get { return emptyFieldsLastNameErrorMessage; }
            set { SetProperty(ref emptyFieldsLastNameErrorMessage, value); }
        }
        public bool ShowEmptyFieldsError
        {
            get { return showEmptyFieldsError; }
            set { SetProperty(ref showEmptyFieldsError, value); }
        }
        public bool IsEmailValid
        {
            get { return _isEmailValid; }
            set { SetProperty(ref _isEmailValid, value); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {

                SetProperty(ref _phoneNumber, value);
            }
        }
        /*setters and getters*/
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }
        private string _birthdate;
        public string Birthdate
        {
            get { return _birthdate; }
            set { SetProperty(ref _birthdate, value); }
        }
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
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
        public string ConfirmPassword
        {
            get { return _confirmpass; }
            set { SetProperty(ref _confirmpass, value); }
        }
        public string SelectedGender
        {
            get => _selectedGender;
            set
            {
                SetProperty(ref _selectedGender, value);

            }
        }
        public new bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }


        public ICommand SignUpCommand { get; set; }

        public SignupViewModel()
        {
            SignUpCommand = new Command(async () => await ExecuteSignUpCommand());
        }

        private bool IsValidEmail(string email)
        {
            // This regular expression pattern is used to check if the email is valid
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }
        private bool IsUsernameValid(string username)
        {
            // Check if username contains numbers or special characters
            if (Regex.IsMatch(username, @"[^\w]|\d"))
            {
                return true;
            }

            return false;
        }
        public bool IsValidPassword(string Password)
        {
            if (Password.Length < 6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private bool ValidateFirstName()
        {
            if (string.IsNullOrEmpty(FirstName) || IsUsernameValid(FirstName))
            {
                EmptyFieldsFirstNameErrorMessage = "Please enter a valid first name without special characters or numbers";
                return false;
            }
            EmptyFieldsFirstNameErrorMessage = "";
            return true;
        }

        private bool ValidateLastName()
        {
            if (string.IsNullOrEmpty(LastName) || IsUsernameValid(LastName))
            {
                EmptyFieldsLastNameErrorMessage = "Please enter a valid last name without special characters or numbers";
                return false;
            }
            EmptyFieldsLastNameErrorMessage = "";
            return true;
        }

        private bool ValidatePhoneNumber()
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                PhoneNumberErrorMessage = "Please enter a valid phone number with a country code.";
                return false;
            }
            PhoneNumberErrorMessage = "";
            return true;
        }

        private bool ValidateEmail()
        {
            if (string.IsNullOrEmpty(Email) || !IsValidEmail(Email))
            {
                EmailErrorMessage = "Please enter a valid email address";
                return false;
            }
            EmailErrorMessage = "";
            return true;
        }

        private bool ValidatePassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                PasswordErrorMessage = "Please enter a password containing at least 6 characters";
                return false;
            }

            if (IsValidPassword(Password))
            {
                PasswordErrorMessage = "Please enter a password containing at least 6 characters";
                return false;
            }

            PasswordErrorMessage = "";
            return true;
        }

        private bool ValidateConfirmPassword()
        {
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                EmptyFieldsErrorMessage = "Please confirm your password";
                return false;
            }

            if (!string.Equals(Password, ConfirmPassword))
            {
                EmptyFieldsErrorMessage = "Passwords do not match";
                return false;
            }

            EmptyFieldsErrorMessage = "";
            return true;
        }

        private bool ValidateBirthdate()
        {
            if (DateTime.TryParse(Birthdate, out DateTime birthDay))
            {
                var today = DateTime.Today;
                var age = today.Year - birthDay.Year;
                if (birthDay > today.AddYears(-age))
                    age--;

                if (age < 16)
                {
                    BirthdateErrorMessage = "You must be at least 16 years old to sign up.";
                    return false;
                }
                BirthdateErrorMessage = "";
                return true;
            }
            else
            {
                BirthdateErrorMessage = "You must be at least 16 years old to sign up.";
                return false;
            }
        }

        private bool ValidateGender()
        {
            if (string.IsNullOrEmpty(_selectedGender))
            {
                GendersErrorMessage = "Please select your gender";
                return false;
            }
            GendersErrorMessage = null;
            return true;
        }
        private bool AllValidationsPassed()
        {
            return ValidateFirstName()
                && ValidateLastName()
                && ValidatePhoneNumber()
                && ValidateEmail()
                && ValidateBirthdate()
                && ValidateGender()
                && ValidatePassword();
        }
        private string DefaultUserPhoto()
        {
            if (_selectedGender == "Male")
            {
                return "MaleDefaultPhoto.png";
            }
            else
            {
                return "FemaleDefaultPhoto.png";
            }
        }
        private async Task CreateUserAccount()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));

                var authResult = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                // Send a verification email
                await authProvider.SendEmailVerificationAsync(authResult.FirebaseToken);
                var newUser = new
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    PhoneNumber = PhoneNumber,
                    Birthdate = Birthdate,
                    Gender = SelectedGender,
                    PhotoUrl = DefaultUserPhoto(),
                };

                await firebaseClient.Child("User").Child(authResult.User.LocalId).PutAsync(newUser);

                await PopupNavigation.Instance.PushAsync(new PopUpSignUp());
            }
            catch (Exception ex)
            {
                EmailErrorMessage = "Account Already Exist With This Email";
            }
        }


        private async Task ExecuteSignUpCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (AllValidationsPassed())
                {
                    await CreateUserAccount();
                }
            }
            catch (FirebaseException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }


}


