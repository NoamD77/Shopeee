using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopeee.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Areas.Identity.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ShopeeeContext _context;
        public ApplicationUserController(ShopeeeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult IsEmailExist([Bind(Prefix = "Input.Email")] string Email)
        {

            return Json(EmailAvailable(Email));

        }
        public bool EmailAvailable(string _Email)
        {
            
            var RegEmail = (from u in _context.Users.ToList()
                            where u.Email.ToUpper().Equals(_Email.ToUpper())
                            select u).FirstOrDefault();

            bool status;
            if (RegEmail != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }


            return status;

        }
        

        [HttpPost]
        public JsonResult IsUserNameExist([Bind(Prefix = "Input.UserName")] string UserName)
        {

            return Json(UserNameAvailable(UserName));

        }
        public bool UserNameAvailable(string _UserName)
        {
            
            var RegUser = (from u in _context.Users.ToList()
                           where u.UserName.ToUpper().Equals(_UserName.ToUpper())
                            select u).FirstOrDefault();

            bool status;
            if (RegUser != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }


            return status;

        }
    }
}
