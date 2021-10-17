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
using Shopeee.Areas.Identity;
using Shopeee.Data;

namespace Shopeee.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public String FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public String LastName { get; set; }
            [Required]
            [Display(Name = "User Name")]
            [Remote("IsUserNameExist", "ApplicationUser", HttpMethod = "POST", ErrorMessage = "Username already registerd.")]
            public String UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            [Remote("IsEmailExist", "ApplicationUser", HttpMethod = "POST", ErrorMessage = "Email already registerd.")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Birth Date")]
            public DateTime BirthDate { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Role { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["roles"] = _roleManager.Roles.ToList();
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var role = _roleManager.FindByIdAsync(Input.Role).Result;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    UserName = Input.UserName,
                    Email = Input.Email,
                    BirthDate = Input.BirthDate,
                    PhoneNumber = Input.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                    if (_signInManager.IsSignedIn(User))
                    {
                        var UserName = await _userManager.GetUserAsync(User);
                        if (await _userManager.IsInRoleAsync(UserName, "Administrator"))
                        {
                            returnUrl = "~/Administrations/UsersList";
                            _logger.LogInformation("Administrator created a new user with password.");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("User created a new user with password.");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                        return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["roles"] = _roleManager.Roles.ToList();
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
