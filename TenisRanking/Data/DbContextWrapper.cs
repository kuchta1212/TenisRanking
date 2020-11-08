using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TenisRanking.Models;

namespace TenisRanking.Data
{
    public class DbContextWrapper : IDbContextWrapper
    {
        private readonly ApplicationDbContext dbContext;

        public DbContextWrapper(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int GetLowestRank()
        {
            return this.dbContext.Players.Any()
                ? this.dbContext.Players.Max(x => x.Rank)
                : 1;
        }

        public List<Player> GetAllPlayers()
        {
            return this.dbContext.Players.OrderBy(x => x.Rank).ToList();
        }

        public Player GetPlayer(string playerId)
        {
            return this.dbContext.Players.FirstOrDefault(x => x.Id == playerId);
        }

        public List<Match> GetAllMatches()
        {
            return this.dbContext.Matches.Include("Result").Where(x => x.Status == MatchStatus.Played).ToList();
        }

        public List<Match> GetAllPlayedMatchesForPlayer(string playerId)
        {
            return this.dbContext.Matches.Include("Result")
                .Where(m => (m.Defender == playerId || m.Chellanger == playerId) && m.Status == MatchStatus.Played)
                .ToList();
        }

        public List<Match> GetAllPlannedMatchesForPlayer(string playerId)
        {
            return this.dbContext.Matches
                .Where(m => (m.Defender == playerId || m.Chellanger == playerId) && m.Status == MatchStatus.Accepted)
                .ToList();
        }

        public List<Match> GetAllChellangedMatchesForPlayer(string playerId)
        {
            return this.dbContext.Matches
                .Where(m => (m.Defender == playerId || m.Chellanger == playerId) && m.Status == MatchStatus.Challanged)
                .ToList();
        }

        public List<Match> GetAllWaitingForConfirmationMatchesForPlayer(string playerId)
        {
            return this.dbContext.Matches.Include("Result")
                .Where(m => (m.Defender == playerId || m.Chellanger == playerId) && m.Status == MatchStatus.WaitingForConfirmation)
                .ToList();
        }

        public List<Match> GetAllRefusedMatches(string playerId)
        {
            return this.dbContext.Matches
                .Where(m => m.Chellanger == playerId && m.Status == MatchStatus.Refused)
                .ToList();
        }

        public void SaveMatch(Match match)
        {
            this.dbContext.Add(match);

            this.dbContext.SaveChanges();
        }

        public void UpdateMatch(Match match)
        {
            this.dbContext.Update(match);

            this.dbContext.SaveChanges();
        }

        public Match GetMatch(string matchId)
        {
            return this.dbContext.Matches.FirstOrDefault(x => x.Id == matchId);
        }

        public void DeleteMatch(string matchId)
        {
            this.dbContext.Remove(matchId);

            this.dbContext.SaveChanges();
        }

        public List<Player> GetPlayersInRanks(int higherRank, int lowerRank, bool includeEdges = false)
        {
            return includeEdges
                ? this.dbContext.Players.Where(p => p.Rank > higherRank && p.Rank <= lowerRank).ToList()
                : this.dbContext.Players.Where(p => p.Rank > higherRank && p.Rank < lowerRank).ToList();
        }

        public void UpdatePlayer(Player player)
        {
            this.dbContext.Update(player);
            this.dbContext.SaveChanges();
        }

        public void ConfirmResult(string matchId)
        {
            var match = this.GetMatch(matchId);
            match.Status = MatchStatus.Played;
            this.dbContext.SaveChanges();
        }
    }
}

