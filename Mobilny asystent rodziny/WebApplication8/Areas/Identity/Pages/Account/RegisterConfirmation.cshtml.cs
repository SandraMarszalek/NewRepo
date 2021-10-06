using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Areas.Identity.Email;

namespace WebApplication8.Areas.Identity.Pages.Account
{
    public class RegisterConfirmation
    {
        [AllowAnonymous]
        public class RegisterConfirmationModel : PageModel
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IEmailSender _sender;

            public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender)
            {
                _userManager = userManager;
                _sender = sender;
            }

            public string Email { get; set; }

            public bool DisplayConfirmAccountLink { get; set; }

            public string EmailConfirmationUrl { get; set; }

            public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
            {

                        if (email == null)
                        {
                            return RedirectToPage("/Index");
                        }

                        var user = await _userManager.FindByEmailAsync(email);
                        if (user == null)
                        {
                            return NotFound($"Unable to load user with email '{email}'.");
                        }

                        Email = email;
                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Email", new { code, email = user.Email }, Request.Scheme);
                        EmailHelper emailHelper = new EmailHelper();
                        bool emailResponse = emailHelper.SendEmail(Email, confirmationLink);
                        if (emailResponse)
                        {
                    return RedirectToPage("./ExternalLogin");// return RedirectToAction("Confirmation","Account");

                        }
                        else
                        {
                            //log email failed
                        }
                return Page();
                    }
            }
        }
    }

