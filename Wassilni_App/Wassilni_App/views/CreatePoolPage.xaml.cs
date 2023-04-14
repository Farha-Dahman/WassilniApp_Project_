using Android.Text.Format;
using Firebase.Auth;
using Java.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var tapGestureRecognizer = new TapGestureRecognizer();
            map.MapClicked += OnMapTapped;

            map.GestureRecognizers.Add(tapGestureRecognizer);
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
           private async void OnMapTapped(object sender, MapClickedEventArgs e)
        {
            var tappedPosition = e.Position;

            var geoCoder = new Geocoder();
            var addresses = await geoCoder.GetAddressesForPositionAsync(tappedPosition);

            if (addresses.Any())
            {
                if (string.IsNullOrWhiteSpace(LocationFrom.Text))
                {
                    LocationFrom.Text = addresses.First();
                }
                else
                {
                    LocationTo.Text = addresses.First();
                }
            }
        }
     
        


        private async Task OnRoutClicked(object sender, EventArgs e)
        {


            try
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

                    // Get route from Google Maps Directions API
                    string apiKey = "AIzaSyCzsoVk0vHyr81imbvoPwSDco1qC6s6WAc";
                    string apiUrl = $"https://maps.googleapis.com/maps/api/directions/json?origin={startPosition.Latitude},{startPosition.Longitude}&destination={endPosition.Latitude},{endPosition.Longitude}&key={apiKey}";

                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetStringAsync(apiUrl);
                        var routeData = JObject.Parse(response);
                        var points = routeData["routes"][0]["overview_polyline"]["points"].ToString();
                        var positions = DecodePolyline(points);

                        // Create a Polyline shape to represent the route between the two locations
                        var polyline = new Polyline();
                        foreach (var position in positions)
                        {
                            polyline.Geopath.Add(position);
                        }
                        polyline.StrokeColor = Color.Blue;
                        polyline.StrokeWidth = 8;

                        // Add the Polyline shape to the map
                        map.MapElements.Add(polyline);

                        // Set the map's region to include both the start and end locations
                        var bounds = new MapSpan(new Position((startPosition.Latitude + endPosition.Latitude) / 2, (startPosition.Longitude + endPosition.Longitude) / 2), Math.Abs(startPosition.Latitude - endPosition.Latitude) * 1.2, Math.Abs(startPosition.Longitude - endPosition.Longitude) * 1.2);
                        map.MoveToRegion(bounds);
                    }


                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }


        private List<Position> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return new List<Position>();

            List<Position> poly = new List<Position>();
            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5Bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                sum = 0;
                shifter = 0;
                do
                {
                    next5Bits = (int)polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                } while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                sum = 0;
                shifter = 0;
                do
                {
                    next5Bits = (int)polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                } while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                poly.Add(new Position(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5));
            }

            return poly;
        }


    }
}