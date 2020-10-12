using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.MatchProvider
{
    public interface IMatchProvider
    {
        void SetFinalMatchResult(string matchId, string firstSetChellanger, string secondSetChellanger, string thirdSetChellanger, string firstSetDefender, string secondSetDefender, string thirdSetDefender);
    }
}
