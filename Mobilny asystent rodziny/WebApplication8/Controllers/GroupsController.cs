using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class GroupsController : Controller
    {

        private readonly WebAppContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupsController(WebAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Dictionary<string, string> GetUsers()
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            users = (from u in _userManager.Users select u).ToList();
            Dictionary<string, string> usersDictionary = new Dictionary<string, string>();
            foreach(ApplicationUser user in users)
            {
                usersDictionary.Add(user.Id, user.User);
            }

            return usersDictionary;
        }

        public List<UserTasks> GetTasks()
        {
             string userId = _userManager.GetUserId(HttpContext.User);
             var userGroup = (from g in _context.RelativesApplicationUsers where g.UserId == userId select g.GroupId).Single();
           /*  List<UserTasks> usersTasks = new List<UserTasks>();
             usersTasks = _context.UserTasks.ToList();
             List<UserTasks> tasksInGroup = new List<UserTasks>();
             foreach(UserTasks task in usersTasks)
             {
                 var userWhoCreatedTask = task.CreatedBy;
                 var userWhoCreatedTaskId = (from u in _userManager.Users where u.User == userWhoCreatedTask select u.Id).Single().ToString();
                 var inWhichGroupThisUserIs = (from r in _context.RelativesApplicationUsers where r.UserId == userWhoCreatedTaskId select r.GroupId).Single().ToString();

                 if (userGroup == inWhichGroupThisUserIs)
                 {
                     tasksInGroup.Add(task);
                 }
             }*/

            List<UserTasks> tasksInGroup = new List<UserTasks>();
            List<UserTasks> allTasks = new List<UserTasks>();
            List<ApplicationUser> users = new List<ApplicationUser>();
            users = _userManager.Users.ToList();
            List<RelativesApplicationUser> usersInGroup = new List<RelativesApplicationUser>();
            usersInGroup = _context.RelativesApplicationUsers.ToList();
            List<string> usersInThisGroupIds = new List<string>();
            foreach (var user in usersInGroup)
            {
                if (user.GroupId == userGroup)
                {
                    usersInThisGroupIds.Add(user.UserId);
                }
            }

            List<string> usersUsername = new List<string>();

            List<string> usersInGroupUsernames = new List<string>();
            foreach (var user in users)
            {
                if (usersInThisGroupIds.Contains(user.Id))
                {
                    usersUsername.Add(user.User);
                }
            }

            allTasks = _context.UserTasks.ToList();
            foreach (UserTasks task in allTasks)
            {
                if (usersUsername.Contains(task.CreatedBy))
                {
                    tasksInGroup.Add(task);
                }
                /*var userWhoCreatedTask = task.CreatedBy;
                var userWhoCreatedTaskId = (from u in _userManager.Users where u.User == userWhoCreatedTask select u.Id).Single().ToString();
                var inWhichGroupThisUserIs = (from r in _context.RelativesApplicationUsers where r.UserId == userWhoCreatedTaskId select r.GroupId).Single().ToString();*/

            }

            return tasksInGroup;
        }
       
        public IActionResult tasksNoGroup()
        {
            return View("tasksNoGroup");
        }

        public IActionResult Tasks()
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

            string userId = _userManager.GetUserId(HttpContext.User);
            if ((from u in _userManager.Users where u.Id == userId select u.FirstName).SingleOrDefault() is null)
            {
                ViewBag.User = (from u in _userManager.Users where u.Id == userId select u.Email).Single().ToString();
            }
            else
            {
                ViewBag.User = (from u in _userManager.Users where u.Id == userId select u.User).Single().ToString();

            }
            ViewBag.loggedInUser = ViewBag.User;

            if ((from x in _context.RelativesApplicationUsers where x.UserId == userId select x.UserId).SingleOrDefault() is null)
            {
                return RedirectToAction("tasksNoGroup");
            }
            dynamic mymodel = new ExpandoObject();
            mymodel.Users = GetUsers();
            mymodel.Tasks = GetTasks();

            return View(mymodel);
        }
    }
}
