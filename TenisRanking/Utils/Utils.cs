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

        //public static bool IsChellangerWinner(int firstSetChellanger, int secondSetChellanger, int thirdSetChellanger, int firstSetDeffender, int secondSetDeffender, int thirdSetDeffender)
        //{
        //    if (firstSetChellanger > firstSetDeffender || secondSetChellanger > secondSetDeffender)
        //    {
        //        if ((thirdSetChellanger == 0 && thirdSetDeffender == 0) || thirdSetChellanger > thirdSetDeffender)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public static string GetSetInString(int challenger, int deffender)
        {
            if (challenger == 0 && deffender == 0)
            {
                return "--";
            }

            return challenger + ":" + deffender;
        }
    }
}
