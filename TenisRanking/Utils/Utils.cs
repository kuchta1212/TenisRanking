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

        public static OpponentState GetOpponentState(Player signInPlayer, Player opponent, List<Match> challengedMatches, List<Match> plannedMatches)
        {
            if (challengedMatches.Any(m => (m.Defender == opponent.Id || m.Chellanger == opponent.Id)))
            {
                return OpponentState.Challenged;
            }

            if (plannedMatches.Any(m => (m.Defender == opponent.Id || m.Chellanger == opponent.Id)))
            {
                return OpponentState.Planned;
            }

            var chellengerLevel = Utils.GetLevel(signInPlayer.Rank);
            var deffenderLevel = Utils.GetLevel(opponent.Rank);


            if ((chellengerLevel - deffenderLevel) == 1 ||
                ((chellengerLevel - deffenderLevel == 0) && signInPlayer.Rank > opponent.Rank))
            {
                return OpponentState.CanChallenged;
            }

            return OpponentState.None;
        }
    }
}
