using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public String UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public String Email { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }



        public String Permissions { get; set; }
    }
}
