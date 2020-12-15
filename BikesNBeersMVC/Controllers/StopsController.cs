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
using System.Security.Claims;
using BikesNBeersMVC.Services.Interfaces;

namespace BikesNBeersMVC.Controllers
{
    public class StopsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStopHandler _stopHandler;

        public StopsController(ApplicationDbContext context,
            IStopHandler stopHandler)
        {
            _context = context;
            _stopHandler = stopHandler;

        }

        // GET: Stop search
        public IActionResult Index(int tripId)
        {
            return View(new StopSearchViewModel() { TripId = tripId } );
        }


        public async Task<IActionResult> SearchForBarOrHotel(StopSearchViewModel requestViewModel)
        {
            List<Stop> stops = null;
            if(string.Equals(requestViewModel.StopType, "brewery", StringComparison.InvariantCultureIgnoreCase))
            {
                stops = await _stopHandler.GetStopByAddress(requestViewModel.AddressStart, requestViewModel.MaxMiles, "brewery");
                stops.Select(stop => stop.TripId = requestViewModel.TripId).ToList();
            }
            else if (string.Equals(requestViewModel.StopType, "hotel", StringComparison.InvariantCultureIgnoreCase))
            {
                stops = await _stopHandler.GetStopByAddress(requestViewModel.AddressStart, requestViewModel.MaxMiles, "hotel");
                stops.Select(stop => stop.TripId = requestViewModel.TripId).ToList();
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
            //the 1st stop always has the starting point and we dont need to reassign to previous stop
            if(trip.Stops.Count > 1)
            {
                var previousStop = trip.Stops[stop.StopOrderNumber - 2];
              //if a user gets this far now our starting points are set correctly
                stop.StartingLatitiude = previousStop.StartingLatitiude;
                stop.StartingLongitude = previousStop.StartingLongitude;
            }

            trip.Stops.Add(stop);

            //before adding a stop to the db we need to (re)calculate the total milages for the trip
            //take the trip.startlat and trip.startlong against the first stop (use stop order)
            //calculate each remaining distance between stops after doing it from the trip start to first stop
            //if we delete a stop later we would do this logic
            trip.TripMiles = await CalculateTripMiles(trip.Stops);

            _context.Add(stop);
            await _context.SaveChangesAsync();
            return View(trip);
        }

        private async Task<double> CalculateTripMiles(List<Stop> stops)
        {
            double totalMilage = 0d;
            foreach (var stop in stops.OrderBy(_ => _.StopOrderNumber))
            {
                double startingLong;
                double startingLat;
                if(stop.StopOrderNumber == 1)
                {
                    var initialLocation = stops.FirstOrDefault(_ => _.StopOrderNumber == 1);
                    startingLong = initialLocation.StartingLongitude;
                    startingLat = initialLocation.StartingLatitiude;
                }
                else
                {
                    startingLong = stop.StartingLongitude;
                    startingLat = stop.StartingLatitiude;
                }
                var stopMilage = await _stopHandler.GetDistance(startingLong, startingLat, stop.lng, stop.lat);
                totalMilage += stopMilage;
            }

            //this may cause loss of precision
            //remove after db update of trip.totalmilage to double
            return totalMilage;
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
