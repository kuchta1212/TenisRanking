using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Data;
using TenisRanking.Models;

namespace TenisRanking.MatchProvider
{
    public class MatchProvider : IMatchProvider
    {
        private readonly IDbContextWrapper context;

        public MatchProvider(IDbContextWrapper context)
        {
            this.context = context;
        }

        public void SetFinalMatchResult(string deffenderId, string challengerId, string matchId, string firstSet, string secondSet, string thirdSet)
        {
            var match = this.context.GetMatch(matchId);

            match.FirstSetDefender = int.Parse(firstSet.Split(":")[0]);
            match.SecondSetDefender = int.Parse(secondSet.Split(":")[0]);

            match.FirstSetChellanger = int.Parse(firstSet.Split(":")[1]);
            match.SecondSetChellanger = int.Parse(secondSet.Split(":")[1]);

            if (!string.IsNullOrEmpty(thirdSet))
            {
                match.ThirdSetDefender = int.Parse(firstSet.Split(":")[0]);
                match.ThirdSetChellanger = int.Parse(secondSet.Split(":")[1]);
            }

            match.DateOfGame = DateTime.Today;
            match.Status = MatchStatus.Played;

            var deffender = this.context.GetPlayer(deffenderId);
            deffender.LastPlayedMatch = DateTime.Today;

            var chellanger = this.context.GetPlayer(challengerId);
            chellanger.LastPlayedMatch = DateTime.Today;

            if (this.IsChellangerWinner(firstSet, secondSet, thirdSet))
            {
                this.AdjustRank(chellanger, deffender);
            }
        }

        private bool IsChellangerWinner(string firstSet, string secondSet, string thirdSet)
        {
            if (this.DoesChallangerWonThisSet(firstSet) || this.DoesChallangerWonThisSet(secondSet))
            {
                if (string.IsNullOrEmpty(thirdSet) || this.DoesChallangerWonThisSet(thirdSet))
                {
                    return true;
                }
            }

            return false;
        }

        private bool DoesChallangerWonThisSet(string set)
        {
            var deffender = int.Parse(set.Split(":")[0]);
            var challenger = int.Parse(set.Split(":")[1]);

            return challenger > deffender && (challenger == 6 || challenger == 7);
        }

        private void AdjustRank(Player challenger, Player deffender)
        {

        }
    }
}
