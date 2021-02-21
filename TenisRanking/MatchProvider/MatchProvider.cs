using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TenisRanking.Data;
using TenisRanking.Job;
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

        public void SetFinalMatchResult(MatchViewModel matchViewModel, string userId)
        {
            var match = this.context.GetMatch(matchViewModel.MatchId);
            match.DateOfGame = DateTime.Today;
            match.Status = MatchStatus.WaitingForConfirmation;

            if (match.Result != null)
            {
                match.Result.CreatedBy = userId;
                this.UpdateSets(matchViewModel, match.Result);
            }
            else
            {
                match.Result = new MatchResult()
                {
                    CreatedBy = userId,
                    Type = matchViewModel.Type,
                    Sets = this.GetSets(matchViewModel)
                };
            }

            match.Result.Winner = this.IsChellangerWinner(match) ? match.Chellanger : match.Defender;
            
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
                    player.LastPlayedMatch = DateTime.Now;

                    this.context.UpdatePlayer(player);
                    playersToMove.ForEach(p => this.context.UpdatePlayer(p));

                }
            }

        }

        public void ConfirmResult(string matchId)
        {
            var match = this.context.GetMatch(matchId);
            var chellanger = this.context.GetPlayer(match.Chellanger);
            var deffender = this.context.GetPlayer(match.Defender);
           
            deffender.LastPlayedMatch = DateTime.Today;
            chellanger.LastPlayedMatch = DateTime.Today;

            this.context.UpdatePlayer(deffender);
            this.context.UpdatePlayer(chellanger);
            match.Status = MatchStatus.Played;

            if (this.IsChellangerWinner(match))
            {
                this.AdjustRank(chellanger, deffender);
            }

            this.context.UpdatePlayer(deffender);
            this.context.UpdatePlayer(chellanger);
            this.context.UpdateMatch(match);
        }

        private bool IsChellangerWinner(Match match)
        {
            return match.Result.Type == MatchResultType.OneSet
                ? this.DidChallangerWinThisSet(match.Result.Sets.First())
                : this.DidChallangerWinThisSet(match.Result.Sets.OrderBy(s => s.Order).ElementAt(1));
        }

        private bool DidChallangerWinThisSet(MatchSet set)
        {
            if (set.Challanger > set.Deffender && (set.ChallengerTieBreak == 0 && set.DeffenderTieBreak == 0))
            {
                return true;
            }

            return set.ChallengerTieBreak > set.DeffenderTieBreak;    
        }

        private List<MatchSet> GetSets(MatchViewModel matchViewModel)
        {
            var sets = new List<MatchSet>
            {
                this.GetSet(matchViewModel.FirstSetChellanger, matchViewModel.FirstSetTieBreakChallanger, matchViewModel.FirstSetDefender, matchViewModel.FirstSetTieBreakDeffender, 1)
            };

            if (matchViewModel.Type == MatchResultType.TwoSets)
            {
                sets.Add(this.GetSet(matchViewModel.SecondSetChellanger, matchViewModel.SecondSetTieBreakChallanger, matchViewModel.SecondSetDefender, matchViewModel.SecondSetTieBreakDeffender, 2));
            }

            return sets;
        }

        private void UpdateSets(MatchViewModel viewModel, MatchResult result)
        {
            this.UpdateSet(viewModel.FirstSetChellanger, viewModel.FirstSetTieBreakChallanger, viewModel.FirstSetDefender, viewModel.FirstSetTieBreakDeffender, result.Sets.OrderBy(s => s.Order).First());
            if (viewModel.Type == MatchResultType.TwoSets)
            {
                this.UpdateSet(viewModel.SecondSetChellanger, viewModel.SecondSetTieBreakChallanger, viewModel.SecondSetDefender, viewModel.SecondSetTieBreakDeffender, result.Sets.OrderBy(s => s.Order).ElementAt(1));
            }
        }

        private void UpdateSet(int challanger, int challengerTieBreak, int deffender, int deffenderTieBreak, MatchSet set)
        {
            set.Challanger = challanger;
            set.Deffender = deffender;
            if (this.WasTieBreakPlayed(challengerTieBreak, deffenderTieBreak))
            {
                set.ChallengerTieBreak = challengerTieBreak;
                set.DeffenderTieBreak = deffenderTieBreak;
            }
            else
            {
                set.ChallengerTieBreak = 0;
                set.DeffenderTieBreak = 0;
            }
        }

        private MatchSet GetSet(int challanger, int challengerTieBreak, int deffender, int deffenderTieBreak, int order)
        {
            return new MatchSet()
            {
                Order = order,
                Challanger = challanger,
                Deffender = deffender,
                ChallengerTieBreak = this.WasTieBreakPlayed(challengerTieBreak, deffenderTieBreak)
                    ? challengerTieBreak
                    : 0,
                DeffenderTieBreak = this.WasTieBreakPlayed(challengerTieBreak, deffenderTieBreak)
                    ? deffenderTieBreak
                    : 0,
            };
        }

        private bool WasTieBreakPlayed(int challenger, int deffender)
        {
            return !(challenger == 0 &&  deffender == 0);
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
