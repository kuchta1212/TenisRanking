using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TenisRanking.Data;
using TenisRanking.Models;

namespace TenisRanking.MatchProvider
{
    public class MatchProvider : IMatchProvider
    {
        private readonly IDbContextWrapper context;
        private readonly IOptions<MatchDaysLimitOptions> daysLimit;

        public MatchProvider(IDbContextWrapper context, IOptions<MatchDaysLimitOptions> daysLimit)
        {
            this.context = context;
            this.daysLimit = daysLimit;
        }

        public void SetFinalMatchResult(string matchId, string firstSetChellanger, string secondSetChellanger, string thirdSetChellanger, string firstSetDefender, string secondSetDefender, string thirdSetDefender)
        {
            var match = this.context.GetMatch(matchId);

            match.FirstSetDefender = int.Parse(firstSetDefender);
            match.SecondSetDefender = int.Parse(secondSetDefender);

            match.FirstSetChellanger = int.Parse(firstSetChellanger);
            match.SecondSetChellanger = int.Parse(secondSetChellanger);

            if (!string.IsNullOrEmpty(thirdSetDefender) && !string.IsNullOrEmpty(thirdSetChellanger))
            {
                match.ThirdSetDefender = int.Parse(thirdSetDefender);
                match.ThirdSetChellanger = int.Parse(thirdSetChellanger);
            }

            match.DateOfGame = DateTime.Today;
            match.Status = MatchStatus.Played;

            var deffender = this.context.GetPlayer(match.Defender);
            deffender.LastPlayedMatch = DateTime.Today;

            var chellanger = this.context.GetPlayer(match.Chellanger);
            chellanger.LastPlayedMatch = DateTime.Today;

            if (this.IsChellangerWinner(match))
            {
                this.AdjustRank(chellanger, deffender);
            }

            this.context.UpdatePlayer(deffender);
            this.context.UpdatePlayer(chellanger);
            this.context.UpdateMatch(match);
        }

        public void CheckDeadlines()
        {
            var players = this.context.GetAllPlayers();

            foreach (var player in players)
            {
                if (player.LastPlayedMatch < DateTime.Now.AddDays(-daysLimit.Value.Days))
                {
                    var level = Utils.Utils.GetLevel(player.Rank);

                    var playersToMove = this.context.GetPlayersInRanks(player.Rank, player.Rank + level +1, true);
                    player.Rank += level +1;
                    playersToMove.ForEach(p => p.Rank--);

                    this.context.UpdatePlayer(player);
                    playersToMove.ForEach(p => this.context.UpdatePlayer(p));

                }
            }

        }

        private bool IsChellangerWinner(Match match)
        {
            if (this.DoesChallangerWonThisSet(match.FirstSetDefender, match.FirstSetChellanger) || this.DoesChallangerWonThisSet(match.FirstSetDefender, match.FirstSetChellanger))
            {
                if ((match.ThirdSetDefender == 0 && match.ThirdSetChellanger == 0) || this.DoesChallangerWonThisSet(match.ThirdSetDefender, match.ThirdSetChellanger))
                {
                    return true;
                }
            }

            return false;
        }

        private bool DoesChallangerWonThisSet(int deffender, int challenger)
        {
            return challenger > deffender && (challenger == 6 || challenger == 7);
        }

        private void AdjustRank(Player challenger, Player deffender)
        {
            var players = this.context.GetPlayersInRanks(deffender.Rank, challenger.Rank);

            challenger.Rank = deffender.Rank;
            deffender.Rank++;

            players.ForEach(p => p.Rank++);

            players.ForEach(p => this.context.UpdatePlayer(p));         
        }
    }
}
