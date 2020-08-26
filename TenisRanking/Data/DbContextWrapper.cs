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

        public Rank GetLowestRank()
        {
            return this.dbContext.Players.Any() 
                ? this.dbContext.Players.Max(x => JsonConvert.DeserializeObject<Rank>(x.Rank)) 
                : new Rank() {Level = 0, Ranking = 1};
        }

        public List<Player> GetAllPlayers()
        {
            return this.dbContext.Players.OrderBy(x => JsonConvert.DeserializeObject<Rank>(x.Rank).Level).ThenBy( x => JsonConvert.DeserializeObject<Rank>(x.Rank).Ranking).ToList();
        }

        public List<Match> GetAllMatches()
        {
            return this.dbContext.Matches.ToList();
        }

        public List<Match> GetAllMatchesForPlayer(Player player)
        {
            return this.dbContext.Matches.Where(m => m.Defender == player.Id || m.Chellanger == player.Id)
                .ToList();
        }
        
    }
}
