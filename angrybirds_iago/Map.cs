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
        [Key]
        public int MapId { get; set; }
        public int Birds { get; set; }

        [Column("MapName", TypeName = "nvarchar")]
        [MaxLength(32)]
        public string MapName { get; set; }

        //Nav prop
        public virtual ICollection<Player> Players { get; set; }
    }
}
