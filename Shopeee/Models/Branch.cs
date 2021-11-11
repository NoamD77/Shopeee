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
        public String City { get; set; }
        public AreaType Area { get; set; }
        public String Location { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public bool Accessibility { get; set; }
        

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public String OpenHours { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public String CloseHours { get; set; }

        public String WorkHours
        {
            get
            {
                return OpenHours + " - " + CloseHours;
            }

        }


    }
}
