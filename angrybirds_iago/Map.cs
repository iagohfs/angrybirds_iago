using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace angrybirds_iago
{
    [Table("MapsTable")]
    public class Map
    {

        public virtual ICollection<Score> Scores { get; set; } // Nav prop

        [Key]
        public int MapId { get; set; }

        [Column("BirdsAvailable", TypeName = "int")]
        public int BirdsAvailable { get; set; }

        [Column("MapName", TypeName = "nvarchar")]
        [MaxLength(32)]
        public string MapName { get; set; }

        public Map() { } // Default const.      


    }
}
