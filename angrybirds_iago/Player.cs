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
        // Nav prop
        public virtual ICollection<Map> Maps { get; set; }
        public virtual ICollection<Score> Scores { get; set; }

        [Key]
        public int PlayerId { get; set; }

        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(32)]
        public string Name { get; set; }        

        public Player() { } // Default const.        

    }

}