using Firebase.Database;
using System;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Wassilni_App.Models;
using Xamarin.Essentials;
using System.Diagnostics;
using Android.App;
using static Android.Views.WindowInsets;

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
                             RequestID=r.Object.RequestID,
                             SelectedGender = r.Object.SelectedGender,
                           

                         })
                        .ToList();
              
                }
        public async Task DeleteRideRequestAsync(string requestID)
        {
            await _firebaseClient.Child("requestRide").Child(requestID).DeleteAsync();
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

        public async Task<Ride> GetBookedRideByRideIdAsync(string rideId)
        {

            
            var Ride = await _firebaseClient
                .Child("Ride")
                .OrderBy("RideID")
                .EqualTo(rideId)
                .OnceAsync<Ride>();
            if (Ride.Count == 0)
            {
                return null;
            }

            var bookedRide = Ride.Select(item => new Ride
            {
                RideID = item.Object.RideID,
             //   RiderID = item.Object.RideID,
                DriverID=item.Object.DriverID,
                CarModel=item.Object.CarModel,
                TripDate=item.Object.TripDate,
                TripTime=item.Object.TripTime,
                PhoneNumber=item.Object.PhoneNumber,
                DriverName = item.Object.DriverName,
                StartLocation = item.Object.StartLocation,
                EndLocation = item.Object.EndLocation,
                PricePerRide = item.Object.PricePerRide,
                PhotoUrl = item.Object.PhotoUrl,
                Date = item.Object.Date,
                PickupDateTime = item.Object.PickupDateTime,
                Number_of_seats=item.Object.Number_of_seats,
                Riders=item.Object.Riders,
             
            }).FirstOrDefault();

            return bookedRide;
        }
        /*
        public async Task<List<Ride>> GetRidesWithRidersByUserIdAsync(string userId)
        {
            var allRides = await _firebaseClient
                .Child("Ride")
                .OnceAsync<Ride>();

            var userRides = allRides
                   .Where(r => r.Object.Riders != null || r.Object.Riders.Any(ri => ri.RiderID == userId))
                .Select(r => new Ride
                {

                    PhotoUrl = r.Object.PhotoUrl,
                    StartLocation = r.Object.StartLocation,
                    EndLocation = r.Object.EndLocation,
                    PickupDateTime = r.Object.PickupDateTime,
                    Number_of_seats = r.Object.Number_of_seats,
                    DriverName = r.Object.DriverName,
                    PhoneNumber = r.Object.PhoneNumber,
                    PricePerRide = r.Object.PricePerRide,
                    DriverID = r.Object.DriverID,
                    TripTime = r.Object.TripTime,
                    Date = r.Object.Date,
                    RideID = r.Object.RideID,
                    CarModel = r.Object.CarModel,
                    TripDate = r.Object.Date.ToString("yyyy-MM-dd"),
                    Riders = r.Object.Riders.ToList(),

                })
                .ToList();
            Debug.WriteLine("Ride list count: " + userRides.Count);


            return userRides;
        }*/
        public async Task<List<Ride>> GetRidesWithRidersByUserIdAsync(string userId)
        {
            var allRides = await _firebaseClient
                .Child("Ride")
                .OnceAsync<Ride>();

            Debug.WriteLine($"All rides count: {allRides.Count}");

            var ridesWithRiders = allRides
                .Where(r => r.Object.Riders != null)
                .ToList();

            Debug.WriteLine($"Rides with riders count: {ridesWithRiders.Count}");

            var userRides = ridesWithRiders
                .Where(r => r.Object.Riders.Any(ri => ri.RiderID==userId))
                .Select(r => new Ride
                {

                    PhotoUrl = r.Object.PhotoUrl,
                    StartLocation = r.Object.StartLocation,
                    EndLocation = r.Object.EndLocation,
                    PickupDateTime = r.Object.PickupDateTime,
                    Number_of_seats = r.Object.Number_of_seats,
                    DriverName = r.Object.DriverName,
                    PhoneNumber = r.Object.PhoneNumber,
                    PricePerRide = r.Object.PricePerRide,
                    DriverID = r.Object.DriverID,
                    TripTime = r.Object.TripTime,
                    Date = r.Object.Date,
                    RideID = r.Object.RideID,
                    CarModel = r.Object.CarModel,
                    TripDate = r.Object.Date.ToString("yyyy-MM-dd"),
                    Riders = r.Object.Riders.ToList(),
                })
                .ToList();

            Debug.WriteLine($"User rides count: {userRides.Count}");

            return userRides;
        }


        public async Task<List<Ride>> LoadRides(string driverId)
        {
            Debug.WriteLine("LoadRides called with driverId: " + driverId);

            try
            {
                var rides = await _firebaseClient
                    .Child("Ride")
                    .OnceAsync<Ride>();

                var rideList = rides
                    .Where(r => r.Object.DriverID == driverId)
                    .Select(r => new Ride
                    {
                        PhotoUrl = r.Object.PhotoUrl,
                        StartLocation = r.Object.StartLocation,
                        EndLocation = r.Object.EndLocation,
                        PickupDateTime = r.Object.PickupDateTime,
                        Number_of_seats = r.Object.Number_of_seats,
                        DriverName = r.Object.DriverName,
                        PhoneNumber = r.Object.PhoneNumber,
                        PricePerRide = r.Object.PricePerRide,
                        DriverID = r.Object.DriverID,
                        TripTime = r.Object.TripTime,
                        Date = r.Object.Date,
                        RideID=r.Object.RideID,   
                        CarModel=r.Object.CarModel,
                        TripDate = r.Object.Date.ToString("yyyy-MM-dd")

            })
                    .ToList();

                Debug.WriteLine("Ride list count: " + rideList.Count);

                return rideList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in LoadRides: " + ex.Message);
                throw;
            }

        }
        public async Task DeleteTripAsync(string tripId)
        {
            try
            {
                await _firebaseClient
                    .Child("Ride")
                    .Child(tripId)
                    .DeleteAsync();

                Debug.WriteLine($"Ride with ID {tripId} deleted successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting ride with ID {tripId}: {ex.Message}");
            }
        }
    }
}