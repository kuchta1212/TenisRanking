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

        List<Match> GetAllPlayedMatchesForPlayer(string playerId);

        List<Match> GetAllPlannedMatchesForPlayer(string playerId);

        List<Match> GetAllChellangedMatchesForPlayer(string playerId);

        List<Match> GetAllRefusedMatches(string playerId);

        List<Match> GetAllWaitingForConfirmationMatchesForPlayer(string playerId);

        void SaveMatch(Match match);

        void UpdateMatch(Match match);

        Match GetMatch(string matchId);

        void DeleteMatch(string matchId);

        List<Player> GetPlayersInRanks(int starterRank, int endRank, bool includeEdges = false);

        void UpdatePlayer(Player player);

        void ConfirmResult(string matchId);
    }
}
