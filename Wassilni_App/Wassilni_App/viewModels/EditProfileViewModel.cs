using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Firebase.Storage;
using Wassilni_App.Models;

namespace Wassilni_App.viewModels
{
    internal class EditProfileViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
        string webAPIkey = "AIzaSyClVyVHgbXooKCTyoKMg6RgfBcnkkFKTX0";
        string userId = Preferences.Get("userId", string.Empty);
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _selectedGender;
        private string _phoneNumber;
        private string _birthdate;
        private string _PersonalPhoto;
        private bool _isBusy;
        private string _emailErrorMessage;
        private string _BirthdateErrorMessage;
        private string _gendersErrorMessage;
        private string _phoneNumberErrorMessage;
        private bool _isEmailValid = true;
        public String _name;
        public String _photoUrl;


        public String PhotoUrl
        {
            get { return _photoUrl; }
            set { SetProperty(ref _photoUrl, value); }
        }
        public String Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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
        public string PhoneNumberErrorMessage
        {
            get => _phoneNumberErrorMessage;
            set => SetProperty(ref _phoneNumberErrorMessage, value);
        }
        public string EmailErrorMessage
        {
            get { return _emailErrorMessage; }
            set { SetProperty(ref _emailErrorMessage, value); }
        }
        public bool IsEmailValid
        {
            get { return _isEmailValid; }
            set { SetProperty(ref _isEmailValid, value); }
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }
        public string personalPhoto
        {
            get { return _PersonalPhoto; }
            set { SetProperty(ref _PersonalPhoto, value); }
        }
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
        public string Birthdate
        {
            get { return _birthdate; }
            set { SetProperty(ref _birthdate, value); }
        }
        public string SelectedGender
        {
            get { return _selectedGender; }
            set { SetProperty(ref _selectedGender, value); }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
            }
        }
        public new bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public async Task GetUser(String UserId)
        {
            var user = await firebaseClient.Child("User").Child(UserId).OnceSingleAsync<Models.User>();
            if (user != null)
            {
                personalPhoto = user.PhotoUrl;
                Name = user.FirstName + " " + user.LastName;
                FirstName = user.FirstName;
                LastName = user.LastName;
                PhoneNumber = "0" + user.PhoneNumber;
                Email = user.Email;
            }
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, pattern);
        }
        private bool IsUsernameValid(string username)
        {
            if (Regex.IsMatch(username, @"[^\w]|\d"))
            {
                return true;
            }
            return false;
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
            if (string.IsNullOrEmpty(SelectedGender))
            {
                GendersErrorMessage = "Please select a gender";
                return false;
            }
            else
            {
                GendersErrorMessage = "";
                return true;
            }
        }
        private bool AllValidationsPassed()
        {
            return ValidateEmail()
                && ValidateBirthdate()
                && ValidatePhoneNumber()
                && ValidateGender();
        }
        public ICommand EditProfilelCommand { get; set; }
        public ICommand EditProfilePhotoCommand { get; set; }

        public EditProfileViewModel()
        {
            string userId = Preferences.Get("userId", string.Empty);
            GetUser(userId);
            EditProfilelCommand = new Command(async () => await ExecuteEditProfilelCommand());
            // EditProfilePhotoCommand = new Command(async () => await ExecuteEditProfilePhotoCommand());

        }
        private async Task ExecuteEditProfilePhotoCommand()
        {


            var permissionResult = await RequestStoragePermission();
            if (!permissionResult)
            {
                return;
            }

            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                await Application.Current.MainPage.DisplayAlert("Error", result.FullPath, "Ok");

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        var storage = new FirebaseStorage("gs://wassilni-app.appspot.com/");
                        var fileName = $"{userId}_{DateTime.Now.Ticks}.jpg";
                        var storagePath = $"user_photos/{fileName}";

                        var uploadTask = storage.Child(storagePath).PutAsync(stream);
                        await uploadTask;

                        var downloadUrl = await storage.Child(storagePath).GetDownloadUrlAsync();
                        personalPhoto = downloadUrl;

                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }


        private async Task<bool> RequestStoragePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status != Xamarin.Essentials.PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            return status == Xamarin.Essentials.PermissionStatus.Granted;
        }





        private async Task ExecuteEditProfilelCommand()
        {
            if (!ValidateEmail() || !ValidateBirthdate() || !ValidatePhoneNumber())
            {
                return;
            }
            var user = await firebaseClient.Child("User").Child(userId).OnceSingleAsync<Models.User>();
            user.Email = Email;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.PhoneNumber = PhoneNumber;
            user.PhotoUrl = personalPhoto;
            user.SelectedGender = SelectedGender;
            user.Birthdate = Birthdate;
            await firebaseClient.Child("User").Child(userId).PutAsync(user);


        }

        private async Task UpdateUserProfile()
        {
            try
            {
                var isModified = false;
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIkey));
                var authResult = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, userId);
                // Send a verification email
                await authProvider.SendEmailVerificationAsync(authResult.FirebaseToken);
                //var personalPhotoUrl = "PersonalPhoto.png";
                var user = await firebaseClient.Child("User").Child(userId).OnceSingleAsync<Models.User>();
                if (!string.Equals(user.FirstName, FirstName))
                {
                    user.FirstName = FirstName;
                    isModified = true;
                }
                if (!string.Equals(user.LastName, LastName))
                {
                    user.LastName = LastName;
                    isModified = true;
                }
                if (!string.Equals(user.Email, Email))
                {
                    user.Email = Email;
                    isModified = true;
                }
                if (!string.Equals(user.PhoneNumber, PhoneNumber))
                {
                    user.PhoneNumber = PhoneNumber;
                    isModified = true;
                }
                if (!string.Equals(user.SelectedGender, SelectedGender))
                {
                    user.SelectedGender = SelectedGender;
                    isModified = true;
                }
                if (!string.Equals(user.Birthdate, Birthdate))
                {
                    user.Birthdate = Birthdate;
                    isModified = true;
                }
                if (!string.IsNullOrEmpty(personalPhoto))
                {
                    await firebaseClient.Child("User").Child(userId).Child("PhotoUrl").PutAsync(personalPhoto);
                }
                if (isModified)
                {
                    await firebaseClient.Child("User").Child(userId).PutAsync(user);
                }
            }
            catch 
            {
                EmailErrorMessage = "Account Already Exist With This Email";
            }
        }
    }
}