using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopeee.Data;
using Shopeee.Models;

namespace Shopeee.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ShopeeeContext _context;

        public ShoppingCartsController(ShopeeeContext context)
        {
            _context = context;
        }

        public class ViewModel
        {
            public List<ShoppingCart> Bag { get; set; }
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index(int id)
        {
            ViewModel Viewbag = new ViewModel();
            //Viewbag.Bag = await _context.ShoppingCart
            //    .Include(s => s.Item)
            //    .Include(s => s.User)
            //    .FirstOrDefaultAsync(m => m.UserId == id);

            //Viewbag.Bag = _context.ShoppingCart
            //    .Include(s => s.Item)
            //    .Include(s => s.User)
            //    .Where(s => s.UserId == id).ToList();

            Viewbag.Bag = (from s in _context.ShoppingCart
                          join i in _context.Item
                          on s.ItemId equals i.Id
                          where s.UserId == id
                          select s).Include(s=>s.Item).ToList();

            //var bagitems = from i in _context.Item
            //               join s in _context.ShoppingCart
            //               on i.Id equals s.ItemId
            //               where s.UserId == id
            //               select i;

            //Viewbag.Bagitems = bagitems.ToList();
            
            //var temp = _context.Item
            //                .Join(_context.ShoppingCart,
            //                      p => p.Id,
            //                      e => e.ItemId,
            //                      (p, e) => e
            //                      ).Where(e => e.UserId == id).FirstOrDefault();
            //var UserExist = _context.User.Find(id);
            //var shopeeeContext = _context.ShoppingCart
            //    .Include(s => s.Item)
            //    .Include(s => s.User);
            
            return View(Viewbag);
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,UserId,ItemId,Quantity")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", shoppingCart.ItemId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", shoppingCart.UserId);
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", shoppingCart.ItemId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", shoppingCart.UserId);
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", shoppingCart.UserId);
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
            return RedirectToAction("Index",new { id = shoppingCartID.UserId });
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.CartID == id);
        }
        
        //[HttpPost]
        //public ActionResult UpdateQuantity(int cartId, int quantity)
        //{
        //    var cart = _context.ShoppingCart.FirstOrDefault(c => c.UserId == UserId);
        //    cart.Quantity = quantity;

        //    _context.SaveChanges();

        //    return RedirectToAction(nameof(Index), new { ID = id });
        //}
    }
}

