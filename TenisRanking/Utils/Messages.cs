using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Operations;

namespace TenisRanking.Models
{
    public static class Messages
    {
        public static string ChallengeSended = nameof(ChallengeSended);

        public static string ChallangeAccpeted = nameof(ChallangeAccpeted);

        public static string ChallengeRefused = nameof(ChallengeRefused);

        public static string MatchRemoved = nameof(MatchRemoved);

        public static string ResultAdded = nameof(ResultAdded);

        public static string ResultConfirmed = nameof(ResultConfirmed);

        public static string ChallangeEmail = nameof(ChallangeEmail);

        public static string ChallangeAcceptedEmail = nameof(ChallangeAcceptedEmail);

        public static string ChallangeRefusedEmail = nameof(ChallangeRefusedEmail);

        public static string ConfirmResultEmail = nameof(ConfirmResultEmail);

        public static string EmailFormat = nameof(EmailFormat);

        public static string RegisterEmail = nameof(RegisterEmail);
    }
}
