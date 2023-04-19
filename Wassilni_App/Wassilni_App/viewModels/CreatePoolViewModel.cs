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
using Wassilni_App.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Views.WindowInsets;
using static Wassilni_App.Models.Ride;

namespace Wassilni_App.viewModels
{
    public class CreatePoolViewModel : BaseViewModel
    {


        FirebaseClient firebaseClient = new Firebase.Database.FirebaseClient("https://wassilni-app-default-rtdb.firebaseio.com/");
    


        private string _fullName;
        private string _phoneNumber;
        private string _startLocation;
        private string _endLocation;
        private DateTime _startDate;
        private TimeSpan _startTime;
        private string _carModel;
        private int _availableSeats;
        private decimal _price;
        public event EventHandler ShowTopErrorMessage;

        private bool _isTopErrorMessageVisible;
        public bool IsTopErrorMessageVisible
        {
            get => _isTopErrorMessageVisible;
            set => SetProperty(ref _isTopErrorMessageVisible, value);
        }
        private string _phoneNumberErrorMessage;
        private string _startLocationErrorMessage;
        private string _endLocationErrorMessage;
        private string _startTimeErrorMessage;
        private string _availableSeatsErrorMessage;
        private string _carModelErrorMessage;
        private string _priceErrorMessage;
        private string _errormessage;
        private string _StartDateErrorMessage;
        


        public string StartDateErrorMessage
        {
            get => _StartDateErrorMessage;
            set => SetProperty(ref _StartDateErrorMessage, value);
        }
        public string RideID { get; set; }

        public TimeSpan Time { get; set; }
        public String DriverGender { get; set; }
        public string TripDate { get; set; }
        public string ErrorMessage
        {
            get => _errormessage;
            set => SetProperty(ref _errormessage, value);
        }

        public string StartLocationErrorMessage
        {
            get => _startLocationErrorMessage;
            set => SetProperty(ref _startLocationErrorMessage, value);
        }

        public string EndLocationErrorMessage
        {
            get => _endLocationErrorMessage;
            set => SetProperty(ref _endLocationErrorMessage, value);
        }

        public string StartTimeErrorMessage
        {
            get => _startTimeErrorMessage;
            set => SetProperty(ref _startTimeErrorMessage, value);
        }

        public string AvailableSeatsErrorMessage
        {
            get => _availableSeatsErrorMessage;
            set => SetProperty(ref _availableSeatsErrorMessage, value);
        }

        public string CarModelErrorMessage
        {
            get => _carModelErrorMessage;
            set => SetProperty(ref _carModelErrorMessage, value);
        }

        public string PriceErrorMessage
        {
            get => _priceErrorMessage;
            set => SetProperty(ref _priceErrorMessage, value);
        }


        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }

        public string StartLocation
        {
            get { return _startLocation; }
            set { SetProperty(ref _startLocation, value); }
        }

        public string EndLocation
        {
            get { return _endLocation; }
            set { SetProperty(ref _endLocation, value); }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        public TimeSpan StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        public string CarModel
        {
            get { return _carModel; }
            set { SetProperty(ref _carModel, value); }
        }

        public int AvailableSeats
        {
            get { return _availableSeats; }
            set { SetProperty(ref _availableSeats, value); }
        }

        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }


        public string PhoneNumberErrorMessage
        {
            get => _phoneNumberErrorMessage;
            set => SetProperty(ref _phoneNumberErrorMessage, value);
        }


        private bool ValidatePhoneNumber()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber) || !Regex.IsMatch(PhoneNumber, @"^[0-9]{8,15}$"))
            {
                return false;
            }

            ErrorMessage = "";
            return true;
        }


        public ICommand CreatePoolCommand { get; set; }

        public CreatePoolViewModel(Ride pool)
        {
            string userId = Preferences.Get("userId", string.Empty);

           
            CreatePoolCommand = new Command(async () => await ExecuteCreatePoolCommand(userId));
           
            //
             FetchUserData(userId,pool);
        }


        private async Task FetchUserData(string userId,Ride pool)
        {
            var user = await firebaseClient.Child("User").Child(userId).OnceSingleAsync<Models.User>();

            if (user != null)
            {
                FullName = $"{user.FirstName} {user.LastName}";
                PhoneNumber = user.PhoneNumber;
                StartLocation = pool.StartLocation;
                EndLocation = pool.EndLocation;
                StartTime = pool.TripTime;
                StartDate = pool.Date;
                AvailableSeats = pool.Number_of_seats;
                DriverGender = user.SelectedGender;
            }
        }
        private bool ValidateStartLocation()
        {
            if (string.IsNullOrEmpty(StartLocation))
            {
                return false;
            }
            return true;
        }

        private bool ValidateEndLocation()
        {
            if (string.IsNullOrEmpty(EndLocation))
            {
                return false;
            }
            return true;
        }
        private bool ValidateStartDate()
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime selectedDateTime = StartDate.Add(StartTime);

            if (selectedDateTime >= currentDateTime)
            {
                return true;
            }
            return false;
        }

        private bool ValidateStartTime()
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime selectedDateTime = StartDate.Add(StartTime);

            if ( selectedDateTime >= currentDateTime)
            {
                return true;
            }
            return false;
        }

        

        private bool ValidateCarModel()
        {
            if (string.IsNullOrEmpty(CarModel))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePrice()
        {
            if (Price <= 0)
            {
                return false;
            }
            return true;
        }

        private bool AllValidationsPassed()
        {
            bool validationResult =
         ValidatePhoneNumber()
         && ValidateStartLocation()
         && ValidateEndLocation()
         && ValidateStartTime()
         && ValidateCarModel()
         && ValidatePrice();

            if (!ValidatePhoneNumber())
            {
                string errorMessage = "Please Enter A valid Phone Number!.";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }

            else if (!ValidateStartLocation())
            {
                string errorMessage = "Please Select A Start Point!.";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }

            else if (!ValidateEndLocation())
            {
                string errorMessage = "Please Selecet Your Distination!";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }
            else if (!ValidateStartTime())
            {
                string errorMessage = "Please Select A Valid Time!.";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }
            else if (!ValidateStartDate())
            {
                string errorMessage = "Please Select A Valid Date!.";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }
           
            else if (!ValidatePrice())
            {
                string errorMessage = "Please Enter The Ride price!.";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }
            else if (!ValidateCarModel())
            {
                string errorMessage = "Please Enter The Ride price!.";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }
            else if (!validationResult)
            {

                string errorMessage = "Please fix the error!.";

                ErrorMessage = errorMessage;
                ShowTopErrorMessage?.Invoke(this, EventArgs.Empty);
            }

            return validationResult;
        }



        private async Task ExecuteCreatePoolCommand(string id)
        {

            string _driverid = id;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (AllValidationsPassed())
                {

                    var personalPhotoUrl = "PersonalPhoto.png";
                    // Create a pool object
                    var newPool = new Ride
                    {
                        PhotoUrl = personalPhotoUrl,
                        DriverID = _driverid,
                        DriverName = FullName,
                        PhoneNumber = PhoneNumber,
                        StartLocation = StartLocation,
                        EndLocation = EndLocation,
                        PickupDateTime = StartDate + StartTime,
                        Date = StartDate,
                        TripTime = StartTime,
                        CarModel = CarModel,
                        Number_of_seats = AvailableSeats,
                        PricePerRide = Price,
                        TripDate = StartDate.ToString("yyyy-MM-dd"),
                        //  DriverGender=DriverGender,

                        Riders = new List<Rider>()

                    };
                    newPool.Riders.Add(new Rider());
                 

                    // Save the pool object to the database
                    var rideReference = await firebaseClient.Child("Ride").PostAsync(newPool);
                    newPool.RideID = rideReference.Key;
                    Preferences.Set("RideID", rideReference.Key);
                    string rideId = Preferences.Get("RideID", string.Empty);
                    newPool.RideID = rideId;
                    await firebaseClient.Child("Ride").Child(rideId).PutAsync(newPool);
                    // Navigate back or display a success message
                    await PopupNavigation.Instance.PushAsync(new PopUpCreatePool());
                }


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
