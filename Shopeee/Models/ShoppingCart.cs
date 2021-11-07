using Shopeee.Areas.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartID { get; set; }
        
        //[ForeignKey("User")]
        //public int UserId { get; set; }
        //public virtual User User { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //One to Many
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }
    }
}