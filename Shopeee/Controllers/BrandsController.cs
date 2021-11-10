using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopeee.Data;
using Shopeee.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Shopeee.Class;

namespace Shopeee.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ShopeeeContext _context;
        private readonly IWebHostEnvironment Environment;
        private GlobalFunctions.GlobalFunctions Functions;

        public BrandsController(ShopeeeContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            Environment = _webHostEnvironment;
            Functions = new GlobalFunctions.GlobalFunctions(_context, Environment);
        }

        // GET: Brands
        public async Task<IActionResult> Index(string? search)
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (search == null)
                {
                    return PartialView(await _context.Brand.ToListAsync());
                }
                else
                {
                    List<Brand> brandsAfterSearch = (from b in _context.Brand
                                                     where b.Name.ToLower().Contains(search.ToLower())
                                                     select b).ToList();
                    return PartialView(brandsAfterSearch);
                }
            }
            else
            {
                return View(await _context.Brand.ToListAsync());
            }
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Logo")] Brand brand, List<IFormFile> postedFiles)
        {
            if (ModelState.IsValid)
            {
                if (postedFiles.Count != 0)
                {
                    IFormFile newUploadedFile = postedFiles[0];
                    string fileExtention = Path.GetExtension(newUploadedFile.FileName);

                    bool check = false;
                    using (var reader = new BinaryReader(newUploadedFile.OpenReadStream()))
                    {
                        var signatures = Signatures._fileSignature[fileExtention];
                        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                        check = signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
                    }
                    if (check)
                    {
                        try
                        {
                            //save images to local folder just for backup
                            Functions.saveImageLocally(newUploadedFile);
                            //.saveImageLocally(newUploadedFile);

                            //upload images to ftp server
                            Functions.UploadPicture(newUploadedFile);

                            brand.Logo = Path.GetFileName(newUploadedFile.FileName);
                        }
                        catch (Exception e)
                        {
                            ViewBag.ErrorMessage = "Connection Timeout";
                            return View(brand);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Not an image";
                        return View(brand);
                    }
                }
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo")] Brand brand, List<IFormFile> postedFiles)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (postedFiles.Count != 0)
                    {
                        IFormFile newUploadedFile = postedFiles[0];
                        //save images to local folder just for backup
                        Functions.saveImageLocally(newUploadedFile);

                        //upload images to ftp server
                        Functions.UploadPicture(newUploadedFile);

                        brand.Logo = Path.GetFileName(newUploadedFile.FileName);
                    }
                    else
                    {
                        var tempbrand = await _context.Brand.FirstOrDefaultAsync(i => i.Id == id);
                        brand.Logo = tempbrand.Logo;
                        _context.Entry(tempbrand).State = EntityState.Detached;
                    }
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brand.FindAsync(id);
            _context.Brand.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _context.Brand.Any(e => e.Id == id);
        }
    }
}
