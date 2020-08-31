using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Models
{
    public class ViewModel
    {
        public List<Player> Players { get; set; }

        public List<Match> PlayedMatches { get; set; }

        public List<Match> PlannedMatches { get; set; }

        public List<Match> RefusedMatches { get; set; }

        public List<Match> ChallengedMatches { get; set; }

        public List<Match> AllMatches { get; set; }
    }
}
