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
    }
}
