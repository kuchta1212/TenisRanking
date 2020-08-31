using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Models
{
    public class PlayerViewModel
    {
        public Player Player { get; set; }

        public List<Match> PlayedMatches { get; set; }

        public List<Match> PlannedMatches { get; set; }

        public List<Match> ChallengedMatches { get; set; }
    }
}
