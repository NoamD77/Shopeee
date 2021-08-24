using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopeee.Models;

namespace Shopeee.Data
{
    public class ShopeeeContext : DbContext
    {
        public ShopeeeContext (DbContextOptions<ShopeeeContext> options)
            : base(options)
        {
        }

        public DbSet<Shopeee.Models.User> User { get; set; }
    }
}
