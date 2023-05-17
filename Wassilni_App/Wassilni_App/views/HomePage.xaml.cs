using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Wassilni_App.viewModels;
using Wassilni_App.views.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Resources;
using Newtonsoft.Json.Linq;

namespace Wassilni_App.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {

            InitializeComponent();
            this.BindingContext = new HomeViewModel();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(32.22307396629019, 35.26089741433303), Distance.FromMiles(1)));
            var tapGestureRecognizer = new TapGestureRecognizer();
            map.MapClicked += OnMapTapped;

            map.GestureRecognizers.Add(tapGestureRecognizer);
            LocationFrom.TextChanged += OnLocationChanged;
            LocationTo.TextChanged += OnLocationChanged;
        }
        private async void OnLocationChanged(object sender, TextChangedEventArgs e)
        {
            var startLocation = LocationFrom.Text;
            var endLocation = LocationTo.Text;

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
                map.Pins.Clear();
                map.MapElements.Clear();

                var startLocation = LocationFrom.Text;
                var endLocation = LocationTo.Text;

                var geoCoder = new Geocoder();
                var startResults = await geoCoder.GetPositionsForAddressAsync(startLocation);
                var endResults = await geoCoder.GetPositionsForAddressAsync(endLocation);

                if (startResults.Any() && endResults.Any())
                {
                    var startPosition = startResults.First();
                    var endPosition = endResults.First();

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

                    string apiKey = "AIzaSyCzsoVk0vHyr81imbvoPwSDco1qC6s6WAc";
                    string apiUrl = $"https://maps.googleapis.com/maps/api/directions/json?origin={startPosition.Latitude},{startPosition.Longitude}&destination={endPosition.Latitude},{endPosition.Longitude}&key={apiKey}&departure_time=now&traffic_model=best_guess";

                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetStringAsync(apiUrl);
                        var routeData = JObject.Parse(response);
                        var points = routeData["routes"][0]["overview_polyline"]["points"].ToString();
                        var positions = DecodePolyline(points);
                        var travelTimeInSeconds = routeData["routes"][0]["legs"][0]["duration"]["value"].Value<int>();
                        TimeSpan travelTime = TimeSpan.FromSeconds(travelTimeInSeconds);
                        string travelTimeString = $"{travelTime.Hours} hours, {travelTime.Minutes} minutes, and {travelTime.Seconds} seconds";
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TravelTimeLabel.Text = $"{"Trip Estimated Time "},{travelTime.Hours}h, {travelTime.Minutes}m,{travelTime.Seconds}s";
                        });


                        var polyline = new Polyline();
                        foreach (var position in positions)
                        {
                            polyline.Geopath.Add(position);
                        }
                        polyline.StrokeColor = Color.Blue;
                        polyline.StrokeWidth = 8;

                        map.MapElements.Add(polyline);

                        var bounds = new MapSpan(new Position((startPosition.Latitude + endPosition.Latitude) / 2, (startPosition.Longitude + endPosition.Longitude) / 2), Math.Abs(startPosition.Latitude - endPosition.Latitude) * 1.2, Math.Abs(startPosition.Longitude - endPosition.Longitude) * 1.2);
                        map.MoveToRegion(bounds);
                    }


                }
            }
            catch 
            {
               
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
          protected override bool OnBackButtonPressed()
        {
            return true;
        }


    }
}


