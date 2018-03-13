using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace angrybirds_iago
{
    [Table("ScoresTable")]
    public class Score
    {
        // Nav prop
        public virtual Player Player { get; set; }
        public virtual Map Map { get; set; }

        [Key]
        public int Id { get; set; }

        [Column("PlayerScore", TypeName = "int")]
        public int PlayerScore { get; set; }

        public Score(Map map, Player player, int pScore)
        {
            Map = map;
            Player = player;
            PlayerScore = pScore;
        }

        public Score() { } // Default const.                

    }
}
