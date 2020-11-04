using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace TenisRanking.Resources
{
    public static class Resource
    {
        public static Dictionary<string, string> Messages  = new Dictionary<string, string>()
        {
            { "ChallangeAccpeted", "Výzva přijata"},
            { "ChallengeRefused", "Výzva odmítnuta"},
            { "ChallengeSended", "Výzva odeslána"},
            { "ResultAdded", "Výsledek přidán"},
            { "MatchRemoved", "Zápas smazán"},
            { "ResultConfirmed", "Výsledek byl potvrzen"}
        };
    }
}
