using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class RelativesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WebAppContext _context;

        public RelativesController(UserManager<ApplicationUser> userManager, WebAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        private static Random random = new Random();
        public string GenerateCode()
        {
            string userId = _userManager.GetUserId(HttpContext.User);

            if (_context.RelativesApplicationUsers.Where(c => c.UserId == userId).Select(c => c.GroupId).SingleOrDefault() == 0)
            {
            codeExists:
                int length = 20;
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string code = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                var result = (from r in _context.Relatives where r.GroupCode == code select r.GroupCode).Count();
                if (result > 0)
                {
                    goto codeExists;
                }

                ViewBag.code = code;
                return ViewBag.code;
            }

            int assignedGroupCode = _context.RelativesApplicationUsers.Where(c => c.UserId == userId).Select(c => c.GroupId).Single();
            ViewBag.code = _context.Relatives.Where(c => c.Id == assignedGroupCode).Select(c => c.GroupCode).Single().ToString();
            return ViewBag.code;
        }

        public async Task<ActionResult> AssignUserToGroup(string groupCode, Relatives model, RelativesApplicationUser modelRelativesUsers)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            if(_context.RelativesApplicationUsers.Where(c => c.UserId == userId).Select(c => c.GroupId).SingleOrDefault().ToString() == "0")
            {
                int groupExists = (from r in _context.Relatives where r.GroupCode == groupCode select r.Id).Count();
                switch (groupExists)
                {
                    case 0:
                        model.GroupCode = groupCode;
                        await _context.AddAsync(model);
                        await _context.SaveChangesAsync();
                        var groupCodeId = (from c in _context.Relatives where c.GroupCode == groupCode select c.Id).Single();
                        modelRelativesUsers.GroupId = groupCodeId;
                        modelRelativesUsers.UserId = userId;
                        _context.Add(modelRelativesUsers);
                        _context.SaveChanges();
                        break;
                    case 1:
                        var groupCodeIdCase1 = (from c in _context.Relatives where c.GroupCode == groupCode select c.Id).Single();
                        modelRelativesUsers.GroupId = groupCodeIdCase1;
                        modelRelativesUsers.UserId = userId;
                        _context.Add(modelRelativesUsers);
                        _context.SaveChanges();
                        break;
                }

                return View("GroupAssigned");

            }
            else
            {
                int groupId = _context.RelativesApplicationUsers.Where(c => c.UserId == userId).Select(c => c.GroupId).Single();
                string usersGroupCode = _context.Relatives.Where(c => c.Id == groupId).Select(c => c.GroupCode).Single().ToString();
                if (groupCode == usersGroupCode)
                {
                    ViewBag.message = "Jesteś już członkiem tej grupy.";
                }
                else
                {
                    ViewBag.message = "Jesteś już członkiem innej grupy! Czy napewno chcesz zmienić grupę?";
                }    
            }

            return RedirectToAction("Index");
        }

        public List<string> getGroupUsers()
        {
            var currentLoggedInUserId = _userManager.GetUserId(HttpContext.User);
            if((from r in _context.RelativesApplicationUsers where r.UserId == currentLoggedInUserId select r.GroupId).Count() == 0)
            {
                List<string> noUsersInGroup = new List<string>();
                ViewBag.usersInGroup = noUsersInGroup;
                return ViewBag.usersInGroup;
            }
            var groupId = (from r in _context.RelativesApplicationUsers where r.UserId == currentLoggedInUserId select r.GroupId).Single();
            List<RelativesApplicationUser> allUsers = new List<RelativesApplicationUser>();
            allUsers = (from r in _context.RelativesApplicationUsers select r).ToList();
            List<string> usersInGroup = new List<string>();
            foreach(RelativesApplicationUser user in allUsers)
            {
                if(user.GroupId == groupId)
                {
                    if ((from u in _userManager.Users where u.Id == currentLoggedInUserId select u.FirstName).SingleOrDefault() is null)
                    {
                        var userNickname = (from u in _userManager.Users where u.Id == currentLoggedInUserId select u.Email).Single().ToString();
                        usersInGroup.Add(userNickname);
                    }
                    else
                    {
                        var userNickname = (from u in _userManager.Users where u.Id == currentLoggedInUserId select u.User).Single().ToString();
                        usersInGroup.Add(userNickname);
                    }                    

                    
                }
            }
            ViewBag.usersInGroup = usersInGroup;
            return ViewBag.usersInGroup;
        }

        public IActionResult NameGroup(string groupName, Relatives relatives)
        {
            var currentLoggedInUserId = _userManager.GetUserId(HttpContext.User);
            var userGroupId = (from g in _context.RelativesApplicationUsers where g.UserId == currentLoggedInUserId select g.GroupId).Single();
            var groupCode = (from c in _context.Relatives where c.Id == userGroupId select c.GroupCode).Single().ToString();
            var thisUserGroupEntity = _context.Relatives.First(r => r.Id == userGroupId);
            if((from r in _context.Relatives where r.Id == userGroupId select r.GroupName).SingleOrDefault() is null)
            {
                if ((from r in _context.Relatives where r.Id == userGroupId select r.GroupName).SingleOrDefault() is null)
                {
                    thisUserGroupEntity.GroupName = groupName;
                    thisUserGroupEntity.GroupCode = groupCode;
                    thisUserGroupEntity.Id = userGroupId;

                    _context.Relatives.Update(thisUserGroupEntity);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.groupName = (from g in _context.Relatives where g.Id == userGroupId select g.GroupName).Single().ToString();
            }
            
            return RedirectToAction("Index");
        }

            public IActionResult Index()
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

            GenerateCode();
            getGroupUsers();
            return View();
        }

        public IActionResult GroupSignInSuccessful()
        {
            return View("GroupAssigned");
        }

    }
}
