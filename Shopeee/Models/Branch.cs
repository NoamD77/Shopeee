using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Location { get; set; }
        public String Address { get; set; }
        

        [DataType(DataType.Time)]
        public int OpenHours { get; set; }

        public List<Item> ItemStock { get; set; }

    }
}
