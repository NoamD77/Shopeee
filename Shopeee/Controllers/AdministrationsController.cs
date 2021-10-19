using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopeee.Areas.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Shopeee.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Shopeee.Models;
using Shopeee.Areas.Identity.Pages.Account;

namespace Shopeee.Controllers
{
    public class AdministrationsController : Controller
    {
        RoleManager<IdentityRole> RoleManager;
        UserManager<ApplicationUser> UserManager;
        private readonly ShopeeeContext _context;

        public AdministrationsController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ShopeeeContext context)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this._context = context;
        }
        public class ViewModel
        {
            public UserManager<ApplicationUser> UserManager { get; set; }
            public RoleManager<IdentityRole> RoleManager { get; set; }
            public ShopeeeContext context { get; set; }
        }

        public class UpdateUserModel
        {
            public string UserId { get; set; }
            public ApplicationUser User { get; set; }
            public RegisterModel.InputModel InputModel { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public RegisterModel Register { get; set; }

        public class InputModel
        {
            public string RoleName { get; set; }
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Index()
        {
            return View(new ViewModel
            {
                UserManager = UserManager,
                RoleManager = RoleManager,
                context = _context
            }); ;
        }


        [Authorize(Policy = "writepolicy")]
        public IActionResult RolesList()
        {
            var roles = RoleManager.Roles.ToList();
            return View(roles);
        }
        [Authorize(Policy = "writepolicy")]
        public IActionResult UsersList()
        {
            var users = UserManager.Users.ToList();
            return View(users);
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("RolesList");
                else
                    Errors(result);
            }
            return View(name);
        }

        [Authorize(Policy = "writepolicy")]
        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            List<ApplicationUser> members = new List<ApplicationUser>();
            List<ApplicationUser> nonMembers = new List<ApplicationUser>();
            List<ApplicationUser> ListOfUsers = UserManager.Users.ToList();
            foreach (ApplicationUser user in ListOfUsers)
            {
                var list = await UserManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            RoleEdit roleEdit = new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
            return View(roleEdit);
        }

        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await UserManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await UserManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction("RolesList");
            else
                return await UpdateRole(model.RoleId);
        }


        [Authorize(Policy = "writepolicy")]
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UpdateUserModel updateUserModel = new UpdateUserModel();
            updateUserModel.User = await UserManager.FindByIdAsync(id);
            updateUserModel.InputModel = new RegisterModel.InputModel();
            return View(updateUserModel);
        }

        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserModel Model)
        {
            var user = await UserManager.FindByIdAsync(Model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                user.FirstName = Model.InputModel.FirstName;
                user.LastName = Model.InputModel.LastName;
                user.BirthDate = Model.InputModel.BirthDate;
                if (!user.Email.Equals(Model.InputModel.Email))
                {
                    var code = await UserManager.GenerateChangeEmailTokenAsync(user, Model.InputModel.Email);
                    await UserManager.ChangeEmailAsync(user, Model.InputModel.Email, code);
                }
                if ("" + user.PhoneNumber == "")
                {
                    await UserManager.SetPhoneNumberAsync(user, Model.InputModel.PhoneNumber);
                }
                else if (!user.PhoneNumber.Equals(Model.InputModel.PhoneNumber))
                {
                    var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user, Model.InputModel.PhoneNumber);
                    await UserManager.ChangePhoneNumberAsync(user, Model.InputModel.PhoneNumber, code);
                }
                if (!Model.InputModel.Password.Equals("NoChange"))
                {
                    await UserManager.RemovePasswordAsync(user);
                    await UserManager.AddPasswordAsync(user, Model.InputModel.Password);
                }
                await UserManager.UpdateAsync(user);
            }


            if (ModelState.IsValid)
                return RedirectToAction("UsersList");
            else
                return await UpdateUser(Model.UserId);
            //return View();
        }


        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("RolesList");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("RolesList", RoleManager.Roles);
        }


        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("UsersList");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No user found");
            return View("UsersList", UserManager.Users);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

    }
}
