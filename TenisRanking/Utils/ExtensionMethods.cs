namespace TenisRanking.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using TenisRanking.Models;

    public static class ExtensionMethods
    {
        public static string MatchStatusToString(this MatchStatus matchStatus)
        {
            switch (matchStatus)
            {
                case MatchStatus.Accepted: return "Naplánováno";
                case MatchStatus.Challanged: return "Vyzváno";
                case MatchStatus.WaitingForConfirmation: return "Čeká se na výsledek";
                case MatchStatus.Refused: return "Odmítnuto";
                default: return matchStatus.ToString();
            }
        }
    }
}
