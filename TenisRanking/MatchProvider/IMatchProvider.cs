using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Models;

namespace TenisRanking.MatchProvider
{
    public interface IMatchProvider
    {
        void SetFinalMatchResult(MatchViewModel matchViewModel, string userId);

        void CheckDeadlines();

        void ConfirmResult(string matchId);
    }
}
