using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BikesNBeersMVC.Context;
using BikesNBeersMVC.Models;
using BikesNBeersMVC.Services;
using BikesNBeersMVC.Services.Interfaces;
using System.Security.Claims;

namespace BikesNBeersMVC.Controllers
{
    public class StopsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBrewHandler _brewHandler;
        private readonly IHotelHandler _hotelHandler;

        public StopsController(ApplicationDbContext context,
            IBrewHandler brewHandler, IHotelHandler hotelHandler)
        {
            _context = context;
            _brewHandler = brewHandler;
            _hotelHandler = hotelHandler;
        }

        // GET: Stop search
        public IActionResult Index(int tripId)
        {
            return View(new StopSearchViewModel() { TripId = tripId } );
        }


        public async Task<IActionResult> SearchForBarOrHotel(StopSearchViewModel requestViewModel)
        {
            var maxMiles = 50;
            if(requestViewModel.MaxMiles != 0)
            {
                maxMiles = requestViewModel.MaxMiles;
            }

            List<Stop> stops = new List<Stop>();

            if(string.Equals(requestViewModel.StopType, "brewery", StringComparison.InvariantCultureIgnoreCase))
            {
                var breweryResponse = await _brewHandler.GetBreweryByAddress(requestViewModel.AddressStart, maxMiles);
                foreach (var brewery in breweryResponse.Results)
                {
                    stops.Add(new Stop
                    {
                        Name = brewery.Name,
                        Rating = brewery.Rating,
                        Address = brewery.Vicinity,
                        //Phone = brewery.Phone
                        Photo = brewery.photoURL,
                        IsHotel = false,
                        //starting lat and starting long are based on the user search not based on the last stop lat or long
                        StartingLatitiude = breweryResponse.StartingLatitude,
                        StartingLongitude = breweryResponse.StartingLongitude,
                        lat = brewery.Geometry.Location.Lat,
                        lng = brewery.Geometry.Location.Lng,
                        TripId = requestViewModel.TripId
                    });
                }
            }
            else if (string.Equals(requestViewModel.StopType, "hotel", StringComparison.InvariantCultureIgnoreCase))
            {
                var hotelResponse = await _hotelHandler.GetHotel(requestViewModel.AddressStart);
                foreach (var hotel in hotelResponse.results)
                {
                    stops.Add(new Stop
                    {
                        Name = hotel.name,
                        Rating = hotel.rating,
                        Address = hotel.vicinity,
                        //Phone = brewery.Phone
                        Photo = hotel.photoURL,
                        IsHotel = true,
                        //starting lat and starting long are based on the user search not based on the last stop lat or long
                        StartingLatitiude = hotelResponse.StartingLatitude,
                        StartingLongitude = hotelResponse.StartingLongitude,
                        lat = hotel.geometry.location.lat,
                        lng = hotel.geometry.location.lng,
                        TripId = requestViewModel.TripId
                    });
                }
            }

            return View("StopSelection", stops);
        }

        public IActionResult StopSelection(List<Stop> stops)
        {
            return View(stops);
        }

        public async Task<IActionResult> SelectedStop(Stop stop)
        {
           
            if (stop.TripId <= 0)
            {
                RedirectToAction("PlanATrip", "Trip");
            }

            var trip = await _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefaultAsync(t => t.Id == stop.TripId);


            stop.StopOrderNumber = trip.Stops.Count + 1;
            if(trip.Stops.Count > 0)
            {
                var previousStop = trip.Stops[stop.StopOrderNumber - 1];
                //if a user gets this far now our starting points are set correctly
                stop.StartingLatitiude = previousStop.StartingLatitiude;
                stop.StartingLongitude = previousStop.StartingLongitude;
            }

            trip.Stops.Add(stop);

            //before adding a stop to the db we need to (re)calculate the total milages for the trip
            //take the trip.startlat and trip.startlong against the first stop (use stop order)
            //calculate each remaining distance between stops after doing it from the trip start to first stop
            //if we delete a stop later we would do this logic



            trip.TripMiles = CalculateTripMiles(trip.Stops);

            _context.Add(stop);
            await _context.SaveChangesAsync();
            return View(trip);
        }

        private int CalculateTripMiles(List<Stop> stops)
        {
            int totalMilage = 0;
            foreach (var stop in stops.OrderBy(_ => _.StopOrderNumber))
            {
                double startingLong;
                double startingLat;
                if(stop.StopOrderNumber == 1)
                {
                    var initialLocation = stops.FirstOrDefault(_ => _.StopOrderNumber == 1);
                   
                }
                else
                {
                    startingLong = stop.StartingLongitude;
                    startingLat = stop.StartingLatitiude;
                }

            //var stopMilage = _backendService.CalculateStopMiles(startingLong, startingLat, endingLong, endingLat);
            // totalMilage += stopMilage;
/*
            http://maps.googleapis.com/maps/api/distancematrix/outputFormat?parameters
            origins = 41.43206,-81.38992 | -33.86748,151.20699

                var httpResponse = await _httpClient.GetAsync($"place/nearbysearch/json?location={coordResults.results[0].geometry.location.lat},{coordResults.results[0].geometry.location.lng}&radius={distance_In_Meters}&keyword=brewery&key=AIzaSyAuKgJKHj3zOAMfx9bGAK8in1s4pYhl0JA");
                httpResponse.EnsureSuccessStatusCode();
                var content = await httpResponse.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<BreweryResponse>(content, _options); */
            }

            return 5000;
            //return totalMilage;
        }

        // GET: Stops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stop = await _context.Stops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stop == null)
            {
                return NotFound();
            }

            return View(stop);
        }

        // GET: Stops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IsHotel,Name,Address,Phone,Photo,lat,lng,StopOrderNumber")] Stop stop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stop);
        }

        // GET: Stops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stop = await _context.Stops.FindAsync(id);
            if (stop == null)
            {
                return NotFound();
            }
            return View(stop);
        }

        // POST: Stops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsHotel,Name,Address,Phone,Photo,lat,lng,StopOrderNumber")] Stop stop)
        {
            if (id != stop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StopExists(stop.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stop);
        }

        // GET: Stops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stop = await _context.Stops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stop == null)
            {
                return NotFound();
            }

            return View(stop);
        }

        // POST: Stops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stop = await _context.Stops.FindAsync(id);
            _context.Stops.Remove(stop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StopExists(int id)
        {
            return _context.Stops.Any(e => e.Id == id);
        }
    }
}
