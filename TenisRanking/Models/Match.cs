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
        public string PlayerOneName { get; set; }

        [ForeignKey("AspNetUsers")]
        public string PlayerTwoName { get; set; }

        public int AmountOfGamesPlayerOne { get; set; }

        public int AmountOfGamesPlayerTwo { get; set; }

        public DateTime DateOfGame { get; set; }
    }
}
