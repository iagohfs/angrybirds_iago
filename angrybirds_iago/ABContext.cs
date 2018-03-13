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
        public DbSet<Score> Scores { get; set; }
        public ABContext() : base("AngryBirds") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var scoreModel = modelBuilder.Entity<Score>();

            scoreModel.HasRequired(s => s.Map)
                .WithMany(m => m.Scores);

            scoreModel.HasRequired(s => s.Player)
                .WithMany(p => p.Scores);            
        }

    }    
}
