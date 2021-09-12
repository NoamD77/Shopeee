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
        //All One to One
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Display(Name = "Total Price")]
        public float TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
