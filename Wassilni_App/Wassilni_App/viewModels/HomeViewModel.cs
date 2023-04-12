using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wassilni_App.Models;
using Wassilni_App.views;
using Xamarin.Forms;

namespace Wassilni_App.viewModels
{
    public class HomeViewModel : BaseViewModel
    {

        private string _locationFrom;
        private string _locationTo;
        private string _EmptyErrorMessage;
        private DateTime _startDate;
        private TimeSpan _startTime;
        private int _numberOfSeats;
        private int _price;


        public int Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        public string LocationFrom
        {
            get { return _locationFrom; }
            set { SetProperty(ref _locationFrom, value); }
        }
        public string EmptyErrorMessage
        {
            get { return _EmptyErrorMessage; }
            set { SetProperty(ref _EmptyErrorMessage, value); }
        }
        public int NumberOfSeats
        {
            get { return _numberOfSeats; }
            set { SetProperty(ref _numberOfSeats, value); }
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
        public string LocationTo
        {
            get { return _locationTo; }
            set { SetProperty(ref _locationTo, value); }
        }

        public ICommand CreatePoolCommand { get; set; }

        public ICommand FindPoolCommand { get; set; }

        public HomeViewModel()
        {
          
            CreatePoolCommand = new Command(async () => await ExecuteCreatePoolCommand());
            FindPoolCommand = new Command(async () => await ExecuteFindPoolCommand());

        }
        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(LocationFrom) || (string.IsNullOrEmpty(LocationTo)))
            {
                EmptyErrorMessage = "Please don't leave any fields empty";
                return false;
            }
            else
            {
                EmptyErrorMessage = "";
                return true;
            }
        }
        private async Task ExecuteCreatePoolCommand()
        {
            try
            {
                if (ValidateFields())
                {
                    Ride Pool = new Ride
                    {
                        StartLocation = LocationFrom,
                        EndLocation = LocationTo,
                        Date = StartDate,
                        Number_of_seats = NumberOfSeats,
                       

                    };


                    await Application.Current.MainPage.Navigation.PushAsync(new CreatePoolPage(Pool));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }

        private async Task ExecuteFindPoolCommand( )
        {
           try {
                if (ValidateFields())
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new FindPoolPage());
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
           
        }
    }


}

