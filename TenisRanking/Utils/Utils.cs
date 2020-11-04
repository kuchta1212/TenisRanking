using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Models;

namespace TenisRanking.Utils
{
    public static class Utils
    {
        public static int GetLevel(int rank)
        {
            var level = 0;
            var addition = 2;
            var checkedRank= 1;
            while (rank > checkedRank)
            {
                checkedRank += addition;
                addition++;
                level++;
            }

            return level;
        }

        public static bool CanChellange(int chellangerRank, int deffenderRank)
        {
            var chellengerLevel = Utils.GetLevel(chellangerRank);
            var deffenderLevel = Utils.GetLevel(deffenderRank);

            return (chellengerLevel - deffenderLevel) == 1 || ((chellengerLevel - deffenderLevel == 0) && deffenderRank < chellangerRank);
        }
    }
}
