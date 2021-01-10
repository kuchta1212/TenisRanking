using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Models;

namespace TenisRanking.Email
{
    public interface IEmailController
    {
        Task<bool> SendChallangeEmail(Player deffender, Player challanger, string matchId);

        Task<bool> SendChallangeAcceptedEmail(Player deffender, Player challanger);

        Task<bool> SendChallangeRefusedEmail(string mailTo, string name, Player deffender, string matchId);

        Task<bool> SendRegisterConfirmationEmail(string to, string name, string subject, string body);

        Task<bool> SendConfirmResultEmail(Player deffender, Player challanger, Match match, bool isReceiverDeffender);
    }
}
