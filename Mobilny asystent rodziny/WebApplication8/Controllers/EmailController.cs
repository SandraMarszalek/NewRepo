using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;

namespace WebApplication8.Controllers
{
    public class EmailController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        public EmailController(UserManager<ApplicationUser> usrMgr)
        {
            userManager = usrMgr;
        }

        public async Task<IActionResult> ConfirmEmail(string code, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");

            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        public async Task<IActionResult> ConfirmEmailAddress(string code, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");

           
            return View("ConfirmEmailAddress");
        }

        public async Task<IActionResult> ResetPassword (string code, string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");

            var result = await userManager.ResetPasswordAsync(user, code, password);
            return View(result.Succeeded ? "./Areas/Pages/Account/NewPassword" : "Error");
        }

        public IActionResult ResetConfirmation()
        {
            return View();
        }
    }
}
