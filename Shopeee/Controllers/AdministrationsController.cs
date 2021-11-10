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
        SignInManager<ApplicationUser> SignInManager;
        RoleManager<IdentityRole> RoleManager;
        UserManager<ApplicationUser> UserManager;
        private readonly ShopeeeContext _context;

        public AdministrationsController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ShopeeeContext context, SignInManager<ApplicationUser> signInManager)
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this.SignInManager = signInManager;
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


            public List<IdentityRole> Member { get; set; }
            public List<IdentityRole> NonMember { get; set; }

            public string[] AddRoles { get; set; }
            public string[] RemoveRoles { get; set; }
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
            });
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
            updateUserModel.Member = new List<IdentityRole>();
            updateUserModel.NonMember = new List<IdentityRole>();
            List<IdentityRole> ListOfRoles = RoleManager.Roles.ToList();
            foreach (IdentityRole role in ListOfRoles)
            {
                var list = await UserManager.IsInRoleAsync(user, role.Name) ? updateUserModel.Member : updateUserModel.NonMember;
                list.Add(role);
            }


            return View(updateUserModel);
        }

        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserModel Model)
        {
            IdentityResult result;
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
                    result = await UserManager.ChangeEmailAsync(user, Model.InputModel.Email, code);
                    if (!result.Succeeded)
                        Errors(result);
                }
                if ("" + user.PhoneNumber == "")
                {
                    await UserManager.SetPhoneNumberAsync(user, Model.InputModel.PhoneNumber);
                }
                else if (!user.PhoneNumber.Equals(Model.InputModel.PhoneNumber))
                {
                    var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user, Model.InputModel.PhoneNumber);
                    result = await UserManager.ChangePhoneNumberAsync(user, Model.InputModel.PhoneNumber, code);
                    if (!result.Succeeded)
                        Errors(result);
                }
                if (!Model.InputModel.Password.Equals("NoChange"))
                {
                    result = await UserManager.RemovePasswordAsync(user);
                    if (!result.Succeeded)
                        Errors(result);
                    result = await UserManager.AddPasswordAsync(user, Model.InputModel.Password);
                    if (!result.Succeeded)
                        Errors(result);
                }

                foreach (string RoleName in Model.AddRoles ?? new string[] { })
                {
                    IdentityRole role = await RoleManager.FindByNameAsync(RoleName);
                    if (role != null)
                    {
                        result = await UserManager.AddToRoleAsync(user, RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string RoleName in Model.RemoveRoles ?? new string[] { })
                {
                    IdentityRole role = await RoleManager.FindByNameAsync(RoleName);
                    if (role != null)
                    {
                        result = await UserManager.RemoveFromRoleAsync(user, RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                await UserManager.UpdateAsync(user);
                await SignInManager.RefreshSignInAsync(user);
            }

            if (ModelState.IsValid)
                return RedirectToAction("UsersList");
            else
                return await UpdateUser(Model.UserId);
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
            ApplicationUser DeletedUser = await UserManager.FindByNameAsync("Deleted-User");
            var UsersOrder = (from s in _context.ShoppingCart
                              where s.UserId == id
                              select s).ToListAsync().Result;
            if (user != null)
            {
                foreach (ShoppingCart shoppingCart in UsersOrder)
                    if (shoppingCart.Ordered)
                        shoppingCart.UserId = DeletedUser.Id;
                    else
                        _context.ShoppingCart.Remove(shoppingCart);
                await _context.SaveChangesAsync();
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

        public IActionResult StatisticsItemsinCarts()
        {
            var ItemsList = _context.Item.ToList();
            var itemidwithcount = (from s in _context.ShoppingCart
                                   group s by s.ItemId into c
                                   select new { ItemId = c.Key, Count = c.Count() });
            List<object> data = new List<object>();
            foreach (Item item in ItemsList)
            {
                var ItemsInCarts = (from r in itemidwithcount
                                    where r.ItemId == item.Id
                                    select r).Count();
                if (ItemsInCarts > 0)
                {
                    ItemsInCarts *= (from s in _context.ShoppingCart
                                     where s.ItemId == item.Id
                                     select s.Quantity).Sum();
                    data.Add(new { Product = item.Name, Count = ItemsInCarts });
                }
            }
            return Json(data);
        }

        public ActionResult StatisticsItemsByBrand()
        {
            var BrandsList = _context.Brand.ToList();
            List<object> data = new List<object>();
            foreach (Brand brand in BrandsList)
            {
                var BrandItemsCount = (from i in _context.Item
                                       where i.BrandId == brand.Id
                                       select i).ToList().Count();
                if (BrandItemsCount > 0)
                    data.Add(new { Product = brand.Name, Count = BrandItemsCount });
            }
            return Json(data);
        }

    }
}
