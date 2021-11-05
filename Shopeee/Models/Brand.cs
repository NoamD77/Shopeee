using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shopeee.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Logo { get; set; }

        [NotMapped]
        public IFormFile TempPicture { get; set; }


        public List<Item> BrandItems { get; set; }
    }
}
