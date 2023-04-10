using Firebase.Database;
using System;
using Firebase.Database.Query;
using Org.Xmlpull.V1.Sax2;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Xamarin.Essentials;
using System.Diagnostics;
namespace Wassilni_App.Services
{
    public class DatabaseHelper
    {
        private readonly FirebaseClient _firebaseClient;
        public DatabaseHelper(string firebaseUrl)
        {
            _firebaseClient = new FirebaseClient(firebaseUrl);
        }
        public async Task<List<Ride>> GetRidesAsync()
        {
            var rides = await _firebaseClient.Child("Ride").OnceAsync<Ride>();
            return rides.Select(r => new Ride
            {
                DriverID = r.Object.DriverID,
                StartLocation = r.Object.StartLocation,
                EndLocation = r.Object.EndLocation,
                PickupDateTime = r.Object.PickupDateTime,
                Number_of_seats = r.Object.Number_of_seats,
                DriverName = r.Object.DriverName,
                PhoneNumber = r.Object.PhoneNumber,
                PricePerRide = r.Object.PricePerRide,
                PhotoUrl = r.Object.PhotoUrl,
            }).ToList();
        }
        public async Task<List<Ride>> GetMatchingPoolsAsync(Ride pool)
        {
            string currentUserId = Preferences.Get("userId", string.Empty);
            var allRides = await GetRidesAsync();

            Debug.WriteLine(pool.Number_of_seats);
            Debug.WriteLine(pool.TripTime);
            Debug.WriteLine(pool.Date);
            Debug.WriteLine(pool.StartLocation);
            Debug.WriteLine(pool.EndLocation);

            pool.PickupDateTime = pool.Date.Add(pool.TripTime);
            Debug.WriteLine(pool.PickupDateTime);


            return allRides.Where(r =>
                r.DriverID != currentUserId &&
                r.StartLocation == pool.StartLocation &&
                r.EndLocation == pool.EndLocation &&
                //r.Date == pool.Date &&
                //r.TripTime == pool.TripTime &&
                //r.PickupDateTime == pool.Date.Add(pool.TripTime) &&
                //r.PickupDateTime == pool.Date.Add(pool.TripTime) &&
                //r.PickupDateTime.TimeOfDay == pool.TripTime //&&
                r.PickupDateTime == pool.PickupDateTime &&
                r.Number_of_seats >= pool.Number_of_seats
               )
                  .Select(r => new Ride
                  {
                      PhotoUrl = r.PhotoUrl,
                      StartLocation = r.StartLocation,
                      EndLocation = r.EndLocation,
                      PickupDateTime = r.PickupDateTime,
                      Number_of_seats = r.Number_of_seats,
                      DriverName = r.DriverName,
                      PhoneNumber = r.PhoneNumber,
                      PricePerRide = r.PricePerRide,
                      DriverID = r.DriverID,
                  })
            .ToList();
        }
    }
}