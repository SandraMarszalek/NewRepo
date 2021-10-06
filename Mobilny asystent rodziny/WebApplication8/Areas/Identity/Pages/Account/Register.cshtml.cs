using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Areas.Identity.Email;
using WebApplication8.Services;
using IEmailSender = WebApplication8.Services.IEmailSender;

namespace WebApplication8.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _sender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender sender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _sender = sender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public string Email { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Pole użytkownik jest wymagane")]
            [DataType(DataType.Text)]
            [Display(Name = "Użytkownik")]
            public string User { get; set; }

            [Required(ErrorMessage = "Pole imię jest wymagane")]
            [DataType(DataType.Text)]
            [Display(Name = "Imię")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Pole email jest wymagane")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Pole hasło jest wymagane")]
            [StringLength(100, ErrorMessage = "Hasło {0} musi mieć przynajmniej {2} znaki oraz maksymalnie {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie. Spróbuj ponownie.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Numer telefonu")]
            public string Phone { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string email, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            email = Input.Email;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user2 = new ApplicationUser { User = Input.User, FirstName = Input.FirstName, UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.Phone };
                var result = await _userManager.CreateAsync(user2, Input.Password);
                if (result.Succeeded)
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
                        return RedirectToAction("ConfirmEmailAddress", "Email", new { email = email});
                    }
                    else
                    {
                        //log email failed
                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        RedirectToAction("RegisterConfirmation", "RegisterConfirmation");
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}