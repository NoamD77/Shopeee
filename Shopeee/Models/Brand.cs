using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Logo { get; set; }


        public List<Item> BrandItems { get; set; }
    }
}
