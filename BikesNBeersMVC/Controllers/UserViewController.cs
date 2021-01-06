using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using BikesNBeersMVC.Context;
using BikesNBeersMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikesNBeersMVC.Controllers
{
 [Authorize]
    public class UserViewController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserViewController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bikerInfo = await _applicationDbContext.BikerInfos.Include(b => b.Trips).Include(b => b.Badges)
                .FirstOrDefaultAsync(biker => biker.UserId == userId);
            return View(bikerInfo);
        }
        public async Task<IActionResult> UpdateUser(int tripId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bikerInfo = await _applicationDbContext.BikerInfos.Include(b => b.Trips).Include(b => b.Badges).FirstOrDefaultAsync(biker => biker.UserId == userId);
            var trips = await _applicationDbContext.Trips.FirstOrDefaultAsync(t => t.Id == tripId);
            double tripMiles = 0.0;
            foreach(var trip in bikerInfo.Trips)
            {
                tripMiles += trip.TripMiles;
            }
            bikerInfo.TotalMiles = (int)tripMiles;
            var success = await CheckBadges(bikerInfo);


            //var startingTotalMiles = bikerInfo.TotalMiles;
            //bikerInfo.TotalMiles +=(int)trips.TripMiles;
            //var roundedMiles = Math.Floor(((double)bikerInfo.TotalMiles / 100)) * 100;
            //if (roundedMiles > startingTotalMiles)
            //{
            //    var newBadge = new Badges()
            //    {
            //        Name = $"{roundedMiles} badge",
            //        Desc = $"badge for passing {roundedMiles}",
            //        MaxMiles = (int)roundedMiles
            //    };
            //    _applicationDbContext.Badges.Add(newBadge);
            //    bikerInfo.Badges.Add(newBadge);
            //    _applicationDbContext.Update(bikerInfo);
            //    await _applicationDbContext.SaveChangesAsync();
            //    //Make new View
            //    return View("Index", bikerInfo);
            //}
            //else
            //{
            //    _applicationDbContext.Update(bikerInfo);
            //    await _applicationDbContext.SaveChangesAsync();

            //}
                _applicationDbContext.Update(bikerInfo);
                await _applicationDbContext.SaveChangesAsync();

            return View("Index", bikerInfo);
        }

        private async Task<bool> CheckBadges(BikerInfo biker)
        {
            var milesTraveled = biker.TotalMiles;
            var milesTraveledRoundedDownToNearestHundred = Math.Floor((double)milesTraveled / 100) * 100;
            var numberOfHundreds = milesTraveledRoundedDownToNearestHundred / 100;
            int hundreds = 0;
            biker.Badges.RemoveAll(_ => _.MaxMiles > milesTraveledRoundedDownToNearestHundred); //trim out any excess from a trip deletion

            for (int i=1; i<=numberOfHundreds; i++)
            {
                hundreds = i * 100;
                if (!biker.Badges.Exists(_=>_.MaxMiles==hundreds))
                {
                    var newBadge = new Badges()
                    {

                        Name = $"{hundreds} badge",
                        Desc = $"badge for passing {hundreds} hundred miles",
                        MaxMiles = hundreds
                    };
                    _applicationDbContext.Badges.Add(newBadge);
                    biker.Badges.Add(newBadge);
               //     _applicationDbContext.Update(newBadge);
                    _applicationDbContext.Update(biker);

                    await _applicationDbContext.SaveChangesAsync();


                }
            }

            return true;
        }
    }
}
