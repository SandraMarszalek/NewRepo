using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Data;
using WebApplication8.Models;
using System.Text.Json;

namespace WebApplication8.Controllers
{
    public class MapController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebAppContext _context;

        public MapController(UserManager<ApplicationUser> userManager, WebAppContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Map()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if ((from u in _userManager.Users where u.Id == userId select u.FirstName).SingleOrDefault() is null)
            {
                ViewBag.User = (from u in _userManager.Users where u.Id == userId select u.Email).Single().ToString();
                ViewData["user"] = (from u in _userManager.Users where u.Id == userId select u.Email).Single().ToString();
            }
            else
            {
                ViewData["user"] = (from u in _userManager.Users where u.Id == userId select u.User).Single().ToString();
                ViewBag.User = (from u in _userManager.Users where u.Id == userId select u.User).Single().ToString();
            }

            ViewBag.loggedInUser = ViewBag.User;
            return View();
        }

        [HttpPost]
        public ActionResult SaveCoordsToDb([FromBody] Map dataCoordsStringify)
        {
            UserMapLocation userLocation = new UserMapLocation();
            Map map = new Map();
            //Map dataCoordsStringify = new Map();

            // var latlongt = System.Text.Json.JsonSerializer.Deserialize<Map>(dataCoordsStringify);
            float lat = dataCoordsStringify.latitude;
            float longt = dataCoordsStringify.longtitude;

            try
            {
                string isThereALoggedInUser = _userManager.GetUserId(HttpContext.User);
                if (DBNull.Value.Equals(isThereALoggedInUser))
                {
                    return View("../Home/Index");
                }
            }
            catch (Exception)
            {

            }

            string userId = _userManager.GetUserId(HttpContext.User);
            if ((from l in _context.UsersMapLocations where l.UserId == userId select l.UserId).SingleOrDefault() is null)
            {
                map.latitude = lat;
                map.longtitude = longt;
                _context.Add(map);
                _context.SaveChanges();
                var maxMapId = (from m in _context.Map select m).ToList();
                List<int> ids = new List<int>();
                foreach (var item in maxMapId)
                {
                    ids.Add(item.Id);
                }
                var max = ids.Last();
                userLocation.UserId = userId;
                userLocation.MapId = max;
                _context.Add(userLocation);
                _context.SaveChanges();
            }
            else
            {
                var mapId = (from l in _context.UsersMapLocations where l.UserId == userId select l.MapId).Single();
                var mapRecord = (from l in _context.Map where l.Id == mapId select l).FirstOrDefault();
                mapRecord.latitude = lat;
                mapRecord.longtitude = longt;
                mapRecord.radius = 0;
                _context.Map.Update(mapRecord);
            }
            
            return RedirectToAction("Map");
        }
    }
}
