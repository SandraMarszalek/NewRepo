using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Data;
using WebApplication8.Models;
using System.Net;
using System.Net.Mail;
using WebApplication8.Resource.Constants;

namespace WebApplication8.Controllers
{
    public class UserTasksController : Controller
    {
        private readonly WebAppContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserTasksController(WebAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserTasks
        public async Task<IActionResult> Index()
        {
            try
            {
                string isThereALoggedInUser = _userManager.GetUserId(HttpContext.User);
                if(DBNull.Value.Equals(isThereALoggedInUser))
                {
                    return View("../Home/Index");
                }
            }
            catch(Exception)
            {

            }

            string userId = _userManager.GetUserId(HttpContext.User);
            if ((from u in _userManager.Users where u.Id == userId select u.FirstName).SingleOrDefault() is null)
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
            ViewBag.loggedInUser = ViewBag.User;

            if ((from x in _context.RelativesApplicationUsers where x.UserId == userId select x.UserId).SingleOrDefault() is null)
            {
                return RedirectToAction("NoGroup");
            }

            var loggedInUserGroup = (from g in _context.RelativesApplicationUsers where g.UserId == userId select g.GroupId).Single();
            if (DBNull.Value.Equals(_context.UserTasks.ToList()))
            {
                return View("IndexNoTasks");
            }
            List<UserTasks> tasksInGroup = new List<UserTasks>();
            List<UserTasks> allTasks = new List<UserTasks>();
            List<ApplicationUser> users = new List<ApplicationUser>();
            users = _userManager.Users.ToList();
            List<RelativesApplicationUser> usersInGroup = new List<RelativesApplicationUser>();
            usersInGroup = _context.RelativesApplicationUsers.ToList();
            List<string> usersInThisGroupIds = new List<string>();
            foreach(var user in usersInGroup)
            {
                if(user.GroupId == loggedInUserGroup)
                {
                    usersInThisGroupIds.Add(user.UserId);
                }
            }

            List<string> usersUsername = new List<string>();

            List<string> usersInGroupUsernames = new List<string>();
            foreach(var user in users)
            {
                if(usersInThisGroupIds.Contains(user.Id))
                {
                    if(user.User is null)
                    {
                        usersUsername.Add(user.Email);
                    }
                    else
                    {
                        usersUsername.Add(user.User);
                    }
                }
            }

            allTasks = _context.UserTasks.ToList();
            foreach (UserTasks task in allTasks)
            {
                if (usersUsername.Contains(task.CreatedBy))
                {
                    tasksInGroup.Add(task);
                }
                
            }

            foreach (Models.UserTasks t in tasksInGroup)
            {
                if (t.WantedUser != null)
                {
                    if ((from u in _userManager.Users where u.Id == t.WantedUser select u.User).Single() is null)
                    {
                        var wantedUserName = (from u in _userManager.Users where u.Id == t.WantedUser select u.Email).Single().ToString();
                        t.WantedUser = wantedUserName;
                    }
                    else
                    {
                        var wantedUserName = (from u in _userManager.Users where u.Id == t.WantedUser select u.User).Single().ToString();
                        t.WantedUser = wantedUserName;
                    }
                    
                }
            }
            return View(tasksInGroup);
        }

        public IActionResult NoGroup()
        {
            return View("IndexNoGroup");
        }

        // GET: UserTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.UserTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tasks.WantedUser != null)
            {
                var wantedUserName = (from u in _userManager.Users where u.Id == tasks.WantedUser select u.User).Single().ToString();
                tasks.WantedUser = wantedUserName;
            }
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        //GET: TaskGroups all
      public List<string> GetGroups()
        {
            List<string> groups = new List<string>();
            groups.Add("Domowe");
            groups.Add("Zakupy");
            groups.Add("Urzędowe");
            groups.Add("Spotkania");
            groups.Add("Wizyty");
            groups.Add("Inne");
            // groups.Insert(0, new TaskGroups { Id = 0, GroupName = "Wybierz grupę" });
            groups.Insert(0, "Wybierz grupę");
            ViewBag.message = groups;
            return ViewBag.message;
        }

        //GET: Users get all to dropdown
        public List<ApplicationUser> GetAllUsersToList()
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            string userId = _userManager.GetUserId(HttpContext.User);
            int groupId = _context.RelativesApplicationUsers.Where(r => r.UserId == userId).Select(r => r.GroupId).Single();
            List<RelativesApplicationUser> usersInGroup = new List<RelativesApplicationUser>();
            usersInGroup = (from r in _context.RelativesApplicationUsers select r).ToList();
            List<string> usersIds = new List<string>();
            foreach (var user in usersInGroup)
            {
                if(user.GroupId == groupId)
                {
                    usersIds.Add(user.UserId);
                }
            }

            users = (from u in _userManager.Users select u).ToList();
            List<ApplicationUser> usersForLoggedInGroup = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (usersIds.Contains((user.Id).ToString()))
                {
                    usersForLoggedInGroup.Add(user);
                }
            }

            usersForLoggedInGroup.Insert(0, new ApplicationUser { Id = "", User = "Wybierz użytkownika"});
            ViewBag.userMessage = usersForLoggedInGroup;
            return ViewBag.usersMessage;
        }

        // GET: UserTasks/Create
        public IActionResult Create()
        {
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
            GetGroups();
            GetAllUsersToList();
            return View();
        }

        //POST: SEND_NOTIFICATION
        public async Task<ActionResult> SendEmailNotification(string wantedUserEmail, string wantedUserName)
        {
            string emailMsg = "Hej " + wantedUserName + ", <br /><br /> zostało Ci przypisane nowe zadanie! <b style='color: red'> Zaloguj się do aplikacji! </b> <br /><br />";
            string emailSubject = EmailInfo.EMAIL_SUBJECT_DEFAULT + "Powiadomienie o nowym zadaniu";

            // Sending Email.  
            await this.SendEmailAsync(wantedUserEmail, emailMsg, emailSubject);


            // Info.  
            return this.Json(new { EnableSuccess = true, SuccessTitle = "Udało się!", SuccessMsg = "Powiadomienie o nowym zadaniu zostało poprawnie wysłane do  '" + wantedUserName + "'" });
        }

        public async Task<bool> SendEmailAsync(string email, string msg, string subject = "")
        {
        // Initialization.  
        bool isSend = false;

        try
        {
            // Initialization.  
            var body = msg;
            var message = new MailMessage();

            // Settings.  
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress(EmailInfo.FROM_EMAIL_ACCOUNT);
            message.Subject = !string.IsNullOrEmpty(subject) ? subject : EmailInfo.EMAIL_SUBJECT_DEFAULT;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                // Settings.  
                var credential = new NetworkCredential
                {
                    UserName = EmailInfo.FROM_EMAIL_ACCOUNT,
                    Password = EmailInfo.FROM_EMAIL_PASSWORD
                };

                // Settings.  
                smtp.Credentials = credential;
                smtp.Host = EmailInfo.SMTP_HOST_GMAIL;
                smtp.Port = Convert.ToInt32(EmailInfo.SMTP_PORT_GMAIL);
                smtp.EnableSsl = true;

                // Sending  
                await smtp.SendMailAsync(message);

                // Settings.  
                isSend = true;
            }
        }
        catch (Exception ex)
        {
            // Info  
            throw ex;
        }

        // info.  
        return isSend;
        }

        // POST: UserTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedAt,EndedAt,CreatedBy,Title,Description,TaskGroup,Status,WantedUser")] UserTasks tasks)
        {
            var dateFrom = DateTime.Now;
            var status = "nowe";
            string userId = _userManager.GetUserId(HttpContext.User);
            if((from u in _userManager.Users where u.Id == userId select u.User).Single() is null)
            {
                string userName = (from u in _userManager.Users where u.Id == userId select u.Email).Single().ToString();
                tasks.CreatedBy = userName;
            }
            else
            {
                string userName = (from u in _userManager.Users where u.Id == userId select u.User).Single().ToString();
                tasks.CreatedBy = userName;

            }
            tasks.CreatedAt = dateFrom;
            tasks.Status = status;

            if (ModelState.IsValid)
            {
                if (tasks.WantedUser != null)
                {
                    string wantedUserEmail = (from u in _userManager.Users where u.Id == tasks.WantedUser select u.Email).Single().ToString();
                    if((from u in _userManager.Users where u.Id == userId select u.User).Single() is null)
                    {
                        string wantedUserName = (from u in _userManager.Users where u.Id == tasks.WantedUser select u.Email).Single().ToString();
                        await this.SendEmailNotification(wantedUserEmail, wantedUserName);
                    }
                    else
                    {
                        string wantedUserName = (from u in _userManager.Users where u.Id == tasks.WantedUser select u.User).Single().ToString();
                        await this.SendEmailNotification(wantedUserEmail, wantedUserName);
                    }
                }

                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(tasks);
        }

        // GET: UserTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetGroups();
            GetAllUsersToList();

            if (id == null)
            {
                return NotFound();
            }
            
            var tasks = await _context.UserTasks.FindAsync(id);

            int taskId = tasks.Id;

            string userId = _userManager.GetUserId(HttpContext.User);
            string userName = (from u in _userManager.Users where u.Id == userId select u.User).Single().ToString();
            string createdBy = (from t in _context.UserTasks where t.Id == taskId select t.CreatedBy).Single().ToString();

            ViewBag.loggedInUser = (from u in _userManager.Users where u.Id == userId select u.User).Single().ToString();

            if (userName != createdBy)
            {
                return RedirectToAction("Index", "UserTasks");
            }

            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        // POST: UserTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedAt,EndedAt,CreatedBy,Title,Description,TaskGroup,Status,WantedUser")] UserTasks tasks)
        {
            if (id != tasks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksExists(tasks.Id))
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
            return View(tasks);
        }

        // GET: UserTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasks = await _context.UserTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasks = await _context.UserTasks.FindAsync(id);
            _context.UserTasks.Remove(tasks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TasksExists(int id)
        {
            return _context.UserTasks.Any(e => e.Id == id);
        }

        public IActionResult InProgress(int? id)
        {
            var ReadyTask = _context.UserTasks.FirstOrDefault(m => m.Id == id);
            ReadyTask.Status = "w trakcie";
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Done(int? id)
        {
            var ReadyTask = _context.UserTasks.FirstOrDefault(m => m.Id == id);
            ReadyTask.Status = "zrobione";
            ReadyTask.EndedAt = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
