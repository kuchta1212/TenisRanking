using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Models;

namespace TenisRanking.Data
{
    public interface IDbContextWrapper
    {
        int GetLowestRank();

        List<Player> GetAllPlayers();

        Player GetPlayer(string playerId);

        List<Match> GetAllMatches();

        List<Match> GetAllMatchesForPlayer(Player player);

        void SaveMatch(Match match);

        Match GetMatch(string matchId);

        void DeleteMatch(string matchId);
    }
}
