using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public class Permissions
    {
        [ForeignKey("User")]
        public int Id { get; set; }
        public UserType Privilege { get; set; }
    }
}
