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
    public class TripController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStopHandler _stopHandler;

        public TripController(ApplicationDbContext context, 
            IStopHandler stopHandler)
        {
            _context = context;
            _stopHandler = stopHandler;
        }

        // GET: Trip
        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips.Include(t => t.BikerInfo).ToListAsync();
            return View(trips);
        }

        public async Task<IActionResult> PlanATrip(StopSearchViewModel welcome)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bikerInfo = await _context.BikerInfos
                .FirstOrDefaultAsync(biker => biker.UserId == userId);

            var trip = new Trip()
            {
                TripName = welcome.TripName,
                TripDate = welcome.TripDate,
                BikerInfoId = bikerInfo.Id
            };

            _context.Add(trip);
            await _context.SaveChangesAsync();

            welcome.TripId = trip.Id;

            //this will need to be wired to go to the stops controller for view
            //this is so we don't have to duplicate the view
            return RedirectToAction("SearchForBarOrHotel", "Stops", welcome);
        }

        [Route("BrewDestGiven")]
        public async Task<IActionResult> PlanATripWithEndDestination(StopSearchViewModel welcome)
        {
            var stops = new List<Stop>();
            //List<Stop> stops = null;
            //if (string.Equals(requestViewModel.StopType, "brewery", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    stops = await _stopHandler.GetStopByAddress(requestViewModel.AddressStart, maxMiles, "brewery");
            //    stops.Select(stop => stop.TripId = requestViewModel.TripId).ToList();
            //}
            //else if (string.Equals(requestViewModel.StopType, "hotel", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    stops = await _stopHandler.GetStopByAddress(requestViewModel.AddressStart, maxMiles, "hotel");
            //    stops.Select(stop => stop.TripId = requestViewModel.TripId).ToList();
            //}


            return View(stops);
        }


        // GET: Trip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.BikerInfo)
                .Include(t => t.Stops)                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // GET: Trip/Create
        public IActionResult Create()
        {
            ViewData["BikerInfoId"] = new SelectList(_context.BikerInfos, "Id", "Id");
            return View();
        }

        // POST: Trip/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TripMiles,TripDate,TripRating,BikerInfoId")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BikerInfoId"] = new SelectList(_context.BikerInfos, "Id", "Id", trip.BikerInfoId);
            return View(trip);
        }

        // GET: Trip/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }
            ViewData["BikerInfoId"] = new SelectList(_context.BikerInfos, "Id", "Id", trip.BikerInfoId);
            return View(trip);
        }

        // POST: Trip/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TripMiles,TripDate,TripRating,BikerInfoId")] Trip trip)
        {
            if (id != trip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return RedirectToAction("Index", "UserView");
            }
            ViewData["BikerInfoId"] = new SelectList(_context.BikerInfos, "Id", "Id", trip.BikerInfoId);
            return View(trip);
        }

        // GET: Trip/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.BikerInfo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "UserView");
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}
