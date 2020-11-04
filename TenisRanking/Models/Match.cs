using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Rewrite.Internal;

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

        public DateTime DateOfGame { get; set; }

        public MatchStatus Status { get; set; }

        [ForeignKey("MatchResult")]
        public MatchResult Result { get; set; }

        public bool IsChallangerWinner()
        {
            return this.Result.Winner == this.Chellanger;
        }
    }
}
