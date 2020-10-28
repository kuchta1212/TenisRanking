using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Models
{
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("AspNetUsers")]
        public string Defender { get; set; }

        [ForeignKey("AspNetUsers")]
        public string Chellanger { get; set; }

        public int FirstSetDefender { get; set; }

        public int SecondSetDefender { get; set; }

        public int ThirdSetDefender { get; set; }

        public int FirstSetChellanger { get; set; }

        public int SecondSetChellanger { get; set; }

        public int ThirdSetChellanger { get; set; }

        public DateTime DateOfGame { get; set; }

        public MatchStatus Status { get; set; }

        public bool IsChellangerWinner()
        {
            if (this.FirstSetChellanger > this.FirstSetDefender || this.SecondSetChellanger > this.SecondSetDefender)
            {
                if ((this.ThirdSetChellanger == 0 && this.ThirdSetDefender == 0) || this.ThirdSetChellanger > this.ThirdSetDefender)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
