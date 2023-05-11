﻿using Firebase.Auth;
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
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using Wassilni_App.Services;
using Android.Content.Res;

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
        private string _driverId;

        public string DriverId
        {
            get { return _driverId; }
            set { SetProperty(ref _driverId, value); }
        }

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
        public ICommand ShowNotificationsCommand { get; }
        public ICommand GoToNotificationsCommand { get; private set; }

        public HomeViewModel()
        {
            GoToNotificationsCommand = new Command(async () => await GoToNotifications());

            CreatePoolCommand = new Command(async () => await ExecuteCreatePoolCommand());
            FindPoolCommand = new Command(async () => await ExecuteFindPoolCommand());
            ShowNotificationsCommand = new Command(async () => await ExecuteShowNotificationsCommand());

        }

        private async Task GoToNotifications()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new NotificationsPage());
        }
        private async Task ExecuteShowNotificationsCommand()
        {
          //  await Application.Current.MainPage.Navigation.PushAsync(new NotificationsPage());
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
                        TripTime= StartTime,
                        Number_of_seats = NumberOfSeats,

                    };


                    await Application.Current.MainPage.Navigation.PushAsync(new NavigationPage(new CreatePoolPage(Pool)));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }

        private async Task ExecuteFindPoolCommand()
        {
            try
            {
                int Number = NumberOfSeats;
                if (ValidateFields())
                {
                    Ride Pool = new Ride
                    {
                        StartLocation = LocationFrom,
                        EndLocation = LocationTo,
                        Date = StartDate,
                        TripTime = StartTime,
                        Number_of_seats = NumberOfSeats,
                        PricePerRide = Price,
                        DriverID = DriverId,
                    };

                    DatabaseHelper dbHelper = ((App)Application.Current).dbHelper;
                    string DriverID = Pool.DriverID;
                    string Number_of_seats = Pool.Number_of_seats.ToString();
                    Preferences.Set("DriverID", DriverId);

                    Preferences.Set("Number_of_seats", Number_of_seats);
                    List<Ride> matchingPools = await dbHelper.GetMatchingPoolsAsync(Pool);
                    await Application.Current.MainPage.Navigation.PushAsync(new FindPoolPage(matchingPools));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}

