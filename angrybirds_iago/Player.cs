using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace angrybirds_iago
{
    [Table("PlayersTable")]
    public class Player
    {
        public Player()
        {
            this.Maps = new HashSet<Map>();
        }

        [Key]
        public int PlayerId { get; set; }

        [Column("PlayerName", TypeName = "nvarchar")]
        [MaxLength(32)]
        public string Name { get; set; }

        public virtual ICollection<Map> Maps { get; set; }
    }

}

//player -< maps, one to many. maps <-> score, one to one.