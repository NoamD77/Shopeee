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
        Shirt = 1,
        Pants = 2,
        Socks = 3,
        Shoes = 4,

    }
}
