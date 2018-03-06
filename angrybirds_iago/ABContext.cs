using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace angrybirds_iago
{
    public class ABContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Map> Maps { get; set; }

        public ABContext() : base("AngryBirds") { }

    }    
}
