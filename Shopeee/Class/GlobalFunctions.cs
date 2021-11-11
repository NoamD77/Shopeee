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

namespace Shopeee.GlobalFunctions
{
    public class GlobalFunctions
    {
        private readonly ShopeeeContext _context;
        private readonly IWebHostEnvironment Environment;

        public GlobalFunctions(ShopeeeContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            Environment = _webHostEnvironment;
        }
        public bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }

        public void saveImageLocally(IFormFile file)
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

        public void UploadPicture(IFormFile file)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(GlobalVariables.ftpImagesPath + Path.GetFileName(file.FileName));
            request.Credentials = new NetworkCredential(GlobalVariables.ftpServerUsername, GlobalVariables.ftpServerPassword);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream ftpStream = request.GetRequestStream())
            {
                file.CopyTo(ftpStream);
            }
        }
    }
}