using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Models;

namespace TenisRanking.Utils
{
    public static class Utils
    {
        public static Rank GetNextRank(Rank rank)
        {
            var maxCountToThisLevel = 0;
            for (var i = 0; i <= rank.Level; i++)
            {
                maxCountToThisLevel += i + 1;
            }

            return maxCountToThisLevel < rank.Ranking + 1 
                ? new Rank() {Level = rank.Level, Ranking = rank.Ranking + 1} 
                : new Rank() {Level = rank.Level + 1, Ranking = rank.Ranking + 1};
        }
    }
}
