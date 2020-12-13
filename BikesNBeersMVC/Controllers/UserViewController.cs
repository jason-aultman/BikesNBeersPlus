using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BikesNBeersMVC.Context;
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
            return View();
        }
    }
}
