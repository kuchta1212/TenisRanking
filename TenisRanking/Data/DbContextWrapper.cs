using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
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
            return this.dbContext.Matches.Where(x => x.Status == MatchStatus.Played).ToList();
        }

        public List<Match> GetAllMatchesForPlayer(Player player)
        {
            return this.dbContext.Matches.Where(m => m.Defender == player.Id || m.Chellanger == player.Id)
                .ToList();
        }

        public List<Match> GetAllPlayedMatchesForPlayer(string playerId)
        {
            return this.GetMatchesForPlayer(playerId, MatchStatus.Played);
        }

        public List<Match> GetAllPlannedMatchesForPlayer(string playerId)
        {
            return this.GetMatchesForPlayer(playerId, MatchStatus.Accepted);
        }

        public List<Match> GetAllChellangedMatchesForPlayer(string playerId)
        {
            return this.GetMatchesForPlayer(playerId, MatchStatus.Chellanged);
        }

        public void SaveMatch(Match match)
        {
            this.dbContext.Add(match);

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

        private List<Match> GetMatchesForPlayer(string playerId, MatchStatus matchStatus)
        {
            return this.dbContext.Matches
                .Where(m => (m.Defender == playerId || m.Chellanger == playerId) && m.Status == matchStatus)
                .ToList();

        }


    }
}
