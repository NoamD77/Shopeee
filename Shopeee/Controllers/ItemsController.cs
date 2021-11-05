using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopeee.Data;
using Shopeee.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System.Text;
using Shopeee.Class;
using Shopeee.GlobalFunctions;

namespace Shopeee.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ShopeeeContext _context;
        private readonly IWebHostEnvironment Environment;
        private GlobalFunctions.GlobalFunctions Functions;

        public ItemsController(ShopeeeContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            Environment = _webHostEnvironment;
            Functions = new GlobalFunctions.GlobalFunctions(_context, Environment);
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var shopeeeContext = _context.Item.Include(i => i.Brand);
            return View(await shopeeeContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {

            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Quantity,Picture,Description,Gender,Type,Color,BrandId")] Item item, List<IFormFile> postedFiles)
        {


            if (ModelState.IsValid)
            {
                if (postedFiles.Count != 0)
                {
                    IFormFile newUploadedFile = postedFiles[0];
                    string ext = Path.GetExtension(newUploadedFile.FileName);
                    //if (newUploadedFile.ContentType.ToLower().StartsWith("image/"))
                    // Check whether the selected file is image
                    //{
                    //    byte[] b;
                    //    using (BinaryReader br = new BinaryReader(newUploadedFile.OpenReadStream()))
                    //    {
                    //        b = br.ReadBytes((int)newUploadedFile.OpenReadStream().Length);
                    //        // Convert the image in to bytes
                    //    }
                    //    Response.StatusCode = 200;
                    //}
                    bool check = false;
                    using (var reader = new BinaryReader(newUploadedFile.OpenReadStream()))
                    {
                        var signatures = Signatures._fileSignature[ext];
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

                            item.Picture = Path.GetFileName(newUploadedFile.FileName);
                        }
                        catch (Exception e)
                        {
                            ViewBag.ErrorMessage = "Connection Timeout";
                            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", item.BrandId);
                            return View(item);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Not an image";
                        ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", item.BrandId);
                        return View(item);
                    }
                }
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", item.BrandId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", item.BrandId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Quantity,Picture,Description,Gender,Type,Color,BrandId")] Item item, List<IFormFile> postedFiles)
        {
            if (id != item.Id)
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

                        item.Picture = Path.GetFileName(newUploadedFile.FileName);
                    }
                    else
                    {
                        var tempitem = await _context.Item.FirstOrDefaultAsync(i => i.Id == id);
                        item.Picture = tempitem.Picture;
                        _context.Entry(tempitem).State = EntityState.Detached;
                    }
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Functions.ItemExists(item.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", item.BrandId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /*
        private StreamReader downloadPicture(string FileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://files.000webhost.com/public_html/images/" + FileName);
            request.Credentials = new NetworkCredential("unthinkable-surface", "Aa123456");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            return reader;
            reader.Close();
            response.Close();
        }*/
    }
}
