using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.MatchProvider
{
    public interface IMatchProvider
    {
        void SetFinalMatchResult(string deffenderId, string challengerId, string matchId, string firstSet, string secondSet, string thirdSet);
    }
}
