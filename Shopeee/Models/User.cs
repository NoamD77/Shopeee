using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Models
{
    public class User
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String eMail { get; set; }
        public UserType Permissions { get; set; }

    }
}
