using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Models
{
    public class MatchSet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public int Challanger { get; set; }

        public int Deffender { get; set; }

        public int ChallengerTieBreak { get; set; }

        public int DeffenderTieBreak { get; set; }

        public override string ToString()
        {
            if (this.ChallengerTieBreak == 0 && this.DeffenderTieBreak == 0)
            {
                return this.Challanger + " : " + this.Deffender;
            }

            return this.Challanger + " : " + this.Deffender + " (" + this.ChallengerTieBreak + " : " + this.DeffenderTieBreak + ")";
        }
    }
}
