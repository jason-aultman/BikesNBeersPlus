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
            //if(selectedBrewView.tripid == nnull || 0)
            //check if we need to create a new trip or not..is the user adding to an existing
            //guard against tripid being 0
            var trip = await _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefaultAsync(t => t.Id == stop.TripId);

            //some check indexing logic for stop number
            //trip.stops.legnth or .count ++ or +1
            //stop.number = the above line

            trip.Stops.Add(stop);
           
            _context.Add(stop);
            await _context.SaveChangesAsync();
            return View(trip);
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
