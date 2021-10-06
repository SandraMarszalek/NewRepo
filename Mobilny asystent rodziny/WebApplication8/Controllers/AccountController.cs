using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;
using System.Security.Claims;

namespace WebApplication8.Controllers
{
    [AllowAnonymous, Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

           [Route("google-login")]
           public IActionResult GoogleLogin()
           {
               var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
               return Challenge(properties, GoogleDefaults.AuthenticationScheme);
           }

           [Route("google-response")]
           public async Task<IActionResult> GoogleResponse()
           {
               var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

               var claims = result.Principal.Identities
                   .FirstOrDefault().Claims.Select(claim => new
                   {
                       claim.Issuer,
                       claim.OriginalIssuer,
                       claim.Type,
                       claim.Value
                   });

               return Json(claims);
           }
           [HttpPost("Account/Confirmation")]
           public async Task<IActionResult> Confirmation(string code, string email)
           {
               var user = await _userManager.FindByEmailAsync(email);
               if (user == null)
                   return View("Error");

               var result = await _userManager.ConfirmEmailAsync(user, code);

               if(result.Succeeded)
               {
                   return RedirectToAction("Profile","User");
               }
               return View( "Error");

           }

        [Route("Account/Logout")]
        public ActionResult Logout()
        {
             _signInManager.SignOutAsync();
            _logger.LogInformation("Wylogowano.");
            
                return View("Logedout");
        }
    }
}
