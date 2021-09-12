using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public enum ItemType
    {
        [Display(Name = "Shirt")]
        Shirt = 0,
        Pants = 1,
        Socks = 2,
        Shoes = 3,
    }
}
