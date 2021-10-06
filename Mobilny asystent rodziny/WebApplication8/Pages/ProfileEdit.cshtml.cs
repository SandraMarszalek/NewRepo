using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication8.Areas.Identity.Data;

namespace WebApplication8.Views.User
{
    [AllowAnonymous]
    public class ProfileEditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileEditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "U¿ytkownik")]
            public string User { get; set; }
            [Display(Name = "Imiê")]
            public string FirstName { get; set; }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var user = (from u in _userManager.Users where u.Id == userId select u).Single();
            if (Input.FirstName != null)
            {
                user.FirstName = Input.FirstName;
            }
            if (Input.User != null)
            {
                user.User = Input.User;
            }
            if (Input.User != null || Input.FirstName != null)
            {
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Profile", "User");

        }
    }
}
