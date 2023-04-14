using Firebase.Database;
using System;
using Firebase.Database.Query;
using Org.Xmlpull.V1.Sax2;
using Org.Apache.Http.Protocol;
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
                TripTime = r.Object.TripTime,
                Date = r.Object.Date,
                RideID=r.Object.RideID
            }).ToList();
        }

        public async Task<List<Ride>> GetMatchingPoolsAsync(Ride pool)
        {
            string currentUserId = Preferences.Get("userId", string.Empty);
            var allRides = await GetRidesAsync();
            pool.PickupDateTime = pool.Date.Add(pool.TripTime);



            return allRides.Where(r =>
                  r.DriverID != currentUserId &&
                  r.StartLocation == pool.StartLocation &&
                  r.EndLocation == pool.EndLocation &&
                  r.PickupDateTime == pool.PickupDateTime &&
                  //((r.PickupDateTime - pool.PickupDateTime).TotalMinutes <= 30) &&
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
                      TripTime = r.TripTime,
                      Date = r.Date,
                      RideID=r.RideID

                  })
            .ToList();
        }
    
       
                public async Task<List<RideRequest>> GetRideRequestsAsync()
                {
                    string currentUserId = Preferences.Get("userId", string.Empty);
                    string DriverID = Preferences.Get("DriverID", string.Empty);

                    var requests = await _firebaseClient
                        .Child("requestRide")
                        .OnceAsync<RideRequest>();

                    return requests
                        .Where(r => r.Object.DriverID == currentUserId)
                         .Select(r => new RideRequest
                         {
                             RequestDate = r.Object.RequestDate,
                             RideID = r.Object.RideID,
                             DriverID = r.Object.DriverID,
                             DriverName = r.Object.DriverName,
                             PhoneNumber = r.Object.PhoneNumber,
                             PhotoUrl = r.Object.PhotoUrl,
                             RiderID = r.Object.RiderID,
                             PickupDateTime = r.Object.PickupDateTime,
                             IsAccepted = r.Object.IsAccepted,
                             RiderName = r.Object.RiderName,
                             StartLocation = r.Object.StartLocation,
                             EndLocation = r.Object.EndLocation,
                             TripTime = r.Object.TripTime,
                             Date = r.Object.Date,
    
                            
                         })
                        .ToList();
              
                }
        public async Task DeleteRideRequestAsync(string requestId)
        {
            await _firebaseClient.Child("requestRide").Child(requestId).DeleteAsync();
        }

        public async Task<string> AddAcceptedTripAsync(BookedRide trip)
        {
            var tripReference = await _firebaseClient.Child("BookedRide").PostAsync(trip);
            return tripReference.Key;
        }
        public async Task<BookedRide> GetAcceptedTripByRideIDAsync(string rideId)
        {
            var acceptedTripList = await _firebaseClient
                .Child("BookedRide")
                .OrderBy("RideID")
                .EqualTo(rideId)
                .OnceAsync<BookedRide>();

            if (acceptedTripList.Count > 0)
            {
                return acceptedTripList.FirstOrDefault().Object;
            }
            else
            {
                return null;
            }
        }
        public async Task UpdateAcceptedTripAsync(BookedRide acceptedTrip)
        {
            if (acceptedTrip == null )
            {
                throw new ArgumentNullException("AcceptedTrip is null or has no ID");
            }

            await _firebaseClient
                .Child("BookedRide")
                .Child(acceptedTrip.TripID)
                .PutAsync(acceptedTrip);
        }

        public async Task<List<BookedRide>> GetBookedRidesByUserIdAsync(string userId)
        {
            var bookedRides = await _firebaseClient
                .Child("BookedRide")
                .OrderBy("RiderID")
                .EqualTo(userId)
                .OnceAsync<BookedRide>();

            return bookedRides.Select(item => new BookedRide
            {
                RideID = item.Object.RideID,
                RiderID = item.Object.RiderID,
                DriverName = item.Object.DriverName,
                RiderName=  item.Object.RiderName,
                StartLocation= item.Object.StartLocation,
                EndLocation = item.Object.EndLocation,
                PricePerRide= item.Object.PricePerRide,
                PhotoUrl=item.Object.PhotoUrl,
                Riders=item.Object.Riders,
            }).ToList();
        }

    }
}