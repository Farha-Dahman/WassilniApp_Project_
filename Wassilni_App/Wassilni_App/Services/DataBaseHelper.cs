using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wassilni_App.Models;

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
                StartLocation = r.Object.StartLocation,
                EndLocation = r.Object.EndLocation,
                PickupDateTime = r.Object.PickupDateTime,
                Number_of_seats = r.Object.Number_of_seats,
                DriverName = r.Object.DriverName,
                PhoneNumber = r.Object.PhoneNumber,
                PricePerRide = r.Object.PricePerRide,
            }).ToList();
        }

        public async Task<List<Ride>> GetMatchingPoolsAsync(Ride pool)
        {
            var allRides = await GetRidesAsync();
            return allRides.Where(r =>
                r.StartLocation == pool.StartLocation &&
                r.EndLocation == pool.EndLocation &&

                r.Number_of_seats >= pool.Number_of_seats
               )
                  .Select(r => new Ride
                  {
                      StartLocation = r.StartLocation,
                      EndLocation = r.EndLocation,
                      PickupDateTime = r.PickupDateTime,
                      Number_of_seats = r.Number_of_seats,
                      DriverName = r.DriverName,
                      PhoneNumber = r.PhoneNumber,
                      PricePerRide = r.PricePerRide
                  })
            .ToList();
        }

    }
}