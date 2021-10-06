using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebAppContext _context;

        public UserController(UserManager<ApplicationUser> userManager, WebAppContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public Task<string> GetCurrentUserId()
        {

            return System.Threading.Tasks.Task.FromResult(ViewBag.userId);
        }

        public List<UserTasks> GetUserTasks()
        {
            ViewBag.userId = _userManager.GetUserId(HttpContext.User);
            string userId = ViewBag.userId;
            List<UserTasks> userTasks = new List<UserTasks>();
            try
            {
                foreach (var x in _context.UserTasks)
                {
                    if (x.WantedUser == userId)
                    {
                        userTasks.Add(x);
                    }
                }
            }
            catch
            {

            }

            return userTasks;
        }
        public IActionResult InProgress(int? id)
        {
            var ReadyTask = _context.UserTasks.FirstOrDefault(m => m.Id == id);
            ReadyTask.Status = "w trakcie";
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        public IActionResult Done(int? id)
        {
            var ReadyTask = _context.UserTasks.FirstOrDefault(m => m.Id == id);
            ReadyTask.Status = "zrobione";
            ReadyTask.EndedAt = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }

        public IActionResult Profile()
        {
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

            ViewBag.userId = _userManager.GetUserId(HttpContext.User);
            string userId = ViewBag.userId;
            var userRecord = _userManager.GetUserId(HttpContext.User);
            if((from u in _userManager.Users where u.Id == userId select u.FirstName).SingleOrDefault() is null)
            {
                ViewBag.Name = "";
                ViewBag.User = (from u in _userManager.Users where u.Id == userId select u.Email).Single().ToString();
            }
            else
            {
                ViewBag.Name = (from u in _userManager.Users where u.Id == userId select u.FirstName).Single().ToString();
                ViewBag.User = (from u in _userManager.Users where u.Id == userId select u.User).Single().ToString();
                
            }
            ViewBag.Email = (from u in _userManager.Users where u.Id == userId select u.Email).Single().ToString();

            if ((from r in _context.RelativesApplicationUsers where r.UserId == userId select r.GroupId).Count() == 0)
            {
                ViewBag.loggedInUser = ViewBag.User;
                return View("ProfileNoGroup");
            }

            ViewBag.loggedInUser = ViewBag.User;

            var groupId = (from r in _context.RelativesApplicationUsers where r.UserId == userId select r.GroupId).Single();
            if ((from r in _context.Relatives where r.Id == groupId select r.GroupName).SingleOrDefault() is null)
            {
                ViewBag.Group = "";
                List<UserTasks> userTask = GetUserTasks();

                return View(userTask);
            }
            ViewBag.loggedInUser = ViewBag.User;
            if((from r in _context.Relatives where r.Id == groupId select r.GroupName).SingleOrDefault() is null)
            {
                ViewBag.Group = "";
            }
            else
            {
                ViewBag.Group = (from r in _context.Relatives where r.Id == groupId select r.GroupName).Single().ToString();
            }
            
            List<UserTasks> userTasks = GetUserTasks();


            return View(userTasks);
        }

        public IActionResult Map()
        {
            return View("Map/Map");
        }

        public IActionResult Groups()
        {
            return View("Groups/Tasks");
        }

        public IActionResult Task()
        {
            return View("UserTasks/Index");
        }

        public List<ApplicationUser> getUser()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<ApplicationUser> user = new List<ApplicationUser>();
            var thisUser = (from u in _userManager.Users where u.Id == userId select u).Single();
            user.Add(thisUser);
            return user;
        }
    }
}
