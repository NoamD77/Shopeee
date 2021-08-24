using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public enum UserType
    {
        [Display(Name = "Client")]
        Client = 1,
        [Display(Name = "Administrator")]
        Administrator = 2,
    }
}
