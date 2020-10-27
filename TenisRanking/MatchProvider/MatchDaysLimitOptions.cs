using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.MatchProvider
{
    public class MatchDaysLimitOptions
    {
        public bool Enabled { get; set; }

        public int Days { get; set; }

        public int LevelDrop { get; set; }

        public int RankDrop { get; set; }
    }
}
