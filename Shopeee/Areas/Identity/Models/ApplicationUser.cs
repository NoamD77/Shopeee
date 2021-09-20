using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Shopeee.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Email")]
        [Remote("IsEmailExist", "ApplicationUser", HttpMethod = "POST", ErrorMessage = "Email already registerd.")]
        public override String Email { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public override String UserName { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
    }
}
