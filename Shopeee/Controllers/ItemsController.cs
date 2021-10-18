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

namespace Shopeee.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ShopeeeContext _context;
        private readonly IWebHostEnvironment Environment;

        public ItemsController(ShopeeeContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            Environment = _webHostEnvironment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var shopeeeContext = _context.Item.Include(i => i.Brand);
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name");
            return View(await shopeeeContext.ToListAsync());
        }

        [HttpPost]
        public PartialViewResult LiveSearch(string search, string type, string color, string gender, string brand)
        {
            List<Item> res = null;
            res = (
                from i in _context.Item
                select i
            ).Include(i => i.Brand).ToList();
            if (search != null) 
            {
                res = res.Where(i => i.Name.Contains(search)).ToList();
            }

            if (type != null)
            {
                res = res.Where(i => i.Type.ToString().ToLower().Equals(type.ToLower())).ToList();
            }

            if (color != null)
            {
                res = res.Where(i => i.Color.ToString().ToLower().Equals(color.ToLower())).ToList();
            }

            if (gender != null)
            {
                res = res.Where(i => i.Gender.ToString().ToLower().Equals(gender.ToLower())).ToList();
            }

            if (brand != null)
            {
                res = res.Where(i => i.Brand.Name.ToString().ToLower().Equals(brand.ToLower())).ToList();
            }

            // Pass the List of results to a Partial View 
            return PartialView("_PartialItemsView", res);
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
            

            if (ModelState.IsValid){
                if (postedFiles.Count != 0)
                {
                    string ext = Path.GetExtension(postedFiles[0].FileName);
                    //if (postedFiles[0].ContentType.ToLower().StartsWith("image/"))
                    // Check whether the selected file is image
                    //{
                    //    byte[] b;
                    //    using (BinaryReader br = new BinaryReader(postedFiles[0].OpenReadStream()))
                    //    {
                    //        b = br.ReadBytes((int)postedFiles[0].OpenReadStream().Length);
                    //        // Convert the image in to bytes
                    //    }
                    //    Response.StatusCode = 200;
                    //}
                    bool check = false;
                    using (var reader = new BinaryReader(postedFiles[0].OpenReadStream()))
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
                        saveImageLocally(postedFiles[0]);

                        //upload images to ftp server
                        uploadPicture(postedFiles[0]);

                        item.Picture = Path.GetFileName(postedFiles[0].FileName);
                        }
                        catch(Exception e)
                        {
                            ViewBag.ErrorMessage = "Connection Timeout";
                            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", item.BrandId);
                            return View(item);
                        }
                    }
                    else { 
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
                        //save images to local folder just for backup
                        saveImageLocally(postedFiles[0]);

                        //upload images to ftp server
                        uploadPicture(postedFiles[0]);

                        item.Picture = Path.GetFileName(postedFiles[0].FileName);
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
                    if (!ItemExists(item.Id))
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

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }

        private byte[] ConvertToBytes(IFormFile image)
        {
            byte[] CoverImageBytes = null;
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            CoverImageBytes = reader.ReadBytes((int)image.Length);
            return CoverImageBytes;
        }

        private void saveImageLocally(IFormFile file)
        {
            string wwwPath = this.Environment.WebRootPath;
            string path = Path.Combine(wwwPath, "imgs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream stream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        private void uploadPicture(IFormFile file)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(GlobalVariables.ftpImagesPath + Path.GetFileName(file.FileName));
            request.Credentials = new NetworkCredential(GlobalVariables.ftpServerUsername, GlobalVariables.ftpServerPassword);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream ftpStream = request.GetRequestStream())
            {
                file.CopyTo(ftpStream);
            }
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
