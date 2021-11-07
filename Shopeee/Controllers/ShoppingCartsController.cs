using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopeee.Data;
using Shopeee.Models;
using Shopeee.Class;
using Shopeee.Services;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Shopeee.Areas.Identity;

namespace Shopeee.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ShopeeeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartsController(ShopeeeContext context, UserManager<ApplicationUser> UserManager)
        {
            _context = context;
            _userManager = UserManager;
        }

        public class ViewModel
        {
            public List<ShoppingCart> Bag { get; set; }
        }

        // GET: ShoppingCarts
        [Authorize(Policy = "readpolicy")]
        public async Task<IActionResult> Index(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == id)
            {
                ViewModel Viewbag = new ViewModel();

                Viewbag.Bag = (from s in _context.ShoppingCart
                               join i in _context.Item
                               on s.ItemId equals i.Id
                               where s.UserId == id
                               select s).Include(s => s.Item).ToList();

                foreach (var item in Viewbag.Bag)
                {
                    item.Item.Price = float.Parse(await Rates.ConvertCurrency(item.Item.Price.ToString()));
                }
                return View(Viewbag);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var shoppingCart = await _context.ShoppingCart
                .Include(s => s.Item)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.CartID == id);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,UserId,ItemId,Quantity")] ShoppingCart shoppingCart)
        {
            var cartItem = (from s in _context.ShoppingCart
                            where shoppingCart.UserId == s.UserId
                            && shoppingCart.ItemId == s.ItemId
                            select s).FirstOrDefault();


            if (ModelState.IsValid)
            {
                var temp = _context.ShoppingCart.Find(shoppingCart.User);
                if (cartItem == null)
                {
                    _context.Add(shoppingCart);
                }
                else
                    cartItem.Quantity += shoppingCart.Quantity;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", shoppingCart.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", shoppingCart.UserId);
            return RedirectToAction("Index", new { id = shoppingCart.UserId });
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", shoppingCart.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", shoppingCart.UserId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartID,UserId,ItemId,Quantity")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.CartID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.CartID))
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", shoppingCart.ItemId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", shoppingCart.UserId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCart
                .Include(s => s.Item)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            var shoppingCartID = await _context.ShoppingCart.FindAsync(id);
            _context.ShoppingCart.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = shoppingCartID.UserId });
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.CartID == id);
        }

        [HttpPost]
        public async Task<string> PostToFacebook(string facebookMassage)
        {
            string FB_PAGE_ID = GlobalVariables.FacebookPageID;
            string FB_ACCESS_TOKEN = GlobalVariables.FacebookPageToken;
            string FB_BASE_ADDRESS = GlobalVariables.FacebookAPI;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(FB_BASE_ADDRESS);

                var parameters = new Dictionary<string, string>
                {
                    { "access_token", FB_ACCESS_TOKEN },
                    { "message", facebookMassage }
                };
                var encodedContent = new FormUrlEncodedContent(parameters);

                var result = await httpClient.PostAsync($"{FB_PAGE_ID}/feed", encodedContent);
                var msg = result.EnsureSuccessStatusCode();
                return await msg.Content.ReadAsStringAsync();
            }
        }

        [HttpPost]
        public ActionResult ChangeRate(string new_Rate)
        {
            GlobalVariables.Rate = new_Rate;
            return new EmptyResult();
        }
    }
}

