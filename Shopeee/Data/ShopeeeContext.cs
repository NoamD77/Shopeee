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

        public DbSet<Shopeee.Models.ShoppingCart> ShoppingCart { get; set; }

        public DbSet<Shopeee.Models.Brand> Brand { get; set; }

        public DbSet<Shopeee.Models.Item> Item { get; set; }

        public DbSet<Shopeee.Models.Branch> Branch { get; set; }

        public DbSet<Shopeee.Models.Permissions> Permissions { get; set; }
    }
}
