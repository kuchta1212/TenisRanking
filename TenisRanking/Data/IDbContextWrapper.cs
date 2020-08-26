using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Models;

namespace TenisRanking.Data
{
    public interface IDbContextWrapper
    {
        Rank GetLowestRank();

        List<Player> GetAllPlayers();

        List<Match> GetAllMatches();

        List<Match> GetAllMatchesForPlayer(Player player);
    }
}
