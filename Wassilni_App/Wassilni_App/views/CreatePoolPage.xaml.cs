using Android.Text.Format;
using Firebase.Auth;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Wassilni_App.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePoolPage : ContentPage
    {
     

      
        public CreatePoolPage(Ride pool)
        {
            InitializeComponent();
           

            this.BindingContext = new CreatePoolViewModel(pool);

            var viewModel = BindingContext as CreatePoolViewModel;
            viewModel.ShowTopErrorMessage += ViewModel_ShowTopErrorMessage;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(32.22307396629019, 35.26089741433303), Distance.FromMiles(1)));
            // Attach the event handlers for the text changed events
            LocationFrom.TextChanged += OnLocationChanged;
            LocationTo.TextChanged += OnLocationChanged;
        }
        private async void ViewModel_ShowTopErrorMessage(object sender, EventArgs e)
        {
            TopErrorFrame.Opacity = 0;
            TopErrorFrame.TranslationY = -30;
            TopErrorFrame.IsVisible = true;

            uint duration = 500;
            uint shakeDuration = 50;
            int shakeCount = 5;
            double shakeOffset = 5;

            await Task.WhenAll(
                TopErrorFrame.FadeTo(1, duration, Easing.CubicInOut),
                TopErrorFrame.TranslateTo(0, 0, duration, Easing.CubicInOut)
            );


            for (int i = 0; i < shakeCount; i++)
            {
                await Task.WhenAll(
                    TopErrorFrame.TranslateTo(shakeOffset, 0, shakeDuration, Easing.Linear),
                    TopErrorFrame.TranslateTo(-shakeOffset, 0, shakeDuration, Easing.Linear)
                );
            }
            await TopErrorFrame.TranslateTo(0, 0, shakeDuration, Easing.Linear);


            await Task.Delay(3000);


            await Task.WhenAll(
                TopErrorFrame.FadeTo(0, duration, Easing.CubicInOut),
                TopErrorFrame.TranslateTo(0, -30, duration, Easing.CubicInOut)
            );
            TopErrorFrame.IsVisible = false;
        }
        private async void OnLocationChanged(object sender, TextChangedEventArgs e)
        {
            // Get the start and end location entries
            var startLocation = LocationFrom.Text;
            var endLocation = LocationTo.Text;

            // Check that both the start and end location entries have values
            if (!string.IsNullOrWhiteSpace(startLocation) && !string.IsNullOrWhiteSpace(endLocation))
            {
                await OnRoutClicked(sender, e);
            }
        }

        private async Task OnRoutClicked(object sender, EventArgs e)
        {
            // Remove any existing pins and polylines from the map
            map.Pins.Clear();
            map.MapElements.Clear();

            // Get the start and end location entries
            var startLocation = LocationFrom.Text;
            var endLocation = LocationTo.Text;

            // Use a geocoder to convert the start and end location into coordinates
            var geoCoder = new Geocoder();
            var startResults = await geoCoder.GetPositionsForAddressAsync(startLocation);
            var endResults = await geoCoder.GetPositionsForAddressAsync(endLocation);

            // Check that we have at least one result for both the start and end location
            if (startResults.Any() && endResults.Any())
            {
                // Get the first result for each location
                var startPosition = startResults.First();
                var endPosition = endResults.First();

                // Add pins for the start and end locations
                map.Pins.Add(new Pin
                {
                    Label = "Start",
                    Position = startPosition,
                    Type = PinType.Place
                });

                map.Pins.Add(new Pin
                {
                    Label = "End",
                    Position = endPosition,
                    Type = PinType.Place
                });

                // Create a Polyline shape to represent the road between the two locations
                var polyline = new Polyline();
                polyline.Geopath.Add(startPosition);
                polyline.Geopath.Add(endPosition);
                polyline.StrokeColor = Color.Blue;
                polyline.StrokeWidth = 5;

                // Add the Polyline shape to the map
                map.MapElements.Add(polyline);

                // Set the map's region to include both the start and end locations
                var bounds = new MapSpan(new Position((startPosition.Latitude + endPosition.Latitude) / 2, (startPosition.Longitude + endPosition.Longitude) / 2), Math.Abs(startPosition.Latitude - endPosition.Latitude) * 1.2, Math.Abs(startPosition.Longitude - endPosition.Longitude) * 1.2);
                map.MoveToRegion(bounds);
            }
        }



    }
}