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
    public class PermissionsController : Controller
    {
        private readonly ShopeeeContext _context;

        public PermissionsController(ShopeeeContext context)
        {
            _context = context;
        }

        // GET: Permissions
        public async Task<IActionResult> Index()
        {
            var shopeeeContext = _context.Permissions.Include(p => p.User);
            return View(await shopeeeContext.ToListAsync());
        }

        // GET: Permissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissions = await _context.Permissions
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (permissions == null)
            {
                return NotFound();
            }

            return View(permissions);
        }

        // GET: Permissions/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Privilege")] Permissions permissions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permissions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", permissions.UserId);
            return View(permissions);
        }

        // GET: Permissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissions = await _context.Permissions.FindAsync(id);
            if (permissions == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", permissions.UserId);
            return View(permissions);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Privilege")] Permissions permissions)
        {
            if (id != permissions.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permissions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionsExists(permissions.UserId))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", permissions.UserId);
            return View(permissions);
        }

        // GET: Permissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissions = await _context.Permissions
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (permissions == null)
            {
                return NotFound();
            }

            return View(permissions);
        }

        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permissions = await _context.Permissions.FindAsync(id);
            _context.Permissions.Remove(permissions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionsExists(int id)
        {
            return _context.Permissions.Any(e => e.UserId == id);
        }
    }
}
