﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wassilni_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Wassilni_App.viewModels;
using static Android.Views.WindowInsets;
using Android.Animation;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindPoolPage : ContentPage
    {
        public ObservableCollection<Ride> MatchingPools { get; set; }
        public FindPoolPage(List<Ride> matchingPools)
        {
            InitializeComponent();
            this.BindingContext = new FindPoolViewModel(matchingPools);
            PoolsCollectionView.ItemsSource = (BindingContext as FindPoolViewModel).MatchingPools;
        }
        /*private async void OnItemTapped(object sender, EventArgs e)
        {
            var tappedFrame = sender as Frame;
            var tappedItem = tappedFrame.BindingContext as Ride;
            Console.WriteLine(tappedFrame.BindingContext);

            await Navigation.PushAsync(new TripDetailsPage(tappedItem));
            

        }*/


        //var tappedItem = tappedFrame.BindingContext as Ride;
        //Console.WriteLine("************************************************");
        //Console.WriteLine(tappedItem.DriverName);

        //if (tappedItem != null)
        //{
        //    await Navigation.PushAsync(new TripDetailsPage(tappedItem.RideID));
        //}
    }
}