using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace angrybirds_iago
{
    [Table("Scores")]
    public class Score
    {
        [Key]
        public int Id { get; set; }
        public int Tries { get; set; }
    }
}
