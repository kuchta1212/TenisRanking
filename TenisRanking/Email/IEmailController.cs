using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Email
{
    public interface IEmailController
    {
        Task<bool> SendChallangeEmail(string mailTo, string name, string challanger, string challengeEmail);

        Task<bool> SendChallangeAcceptedEmail(string mailTo, string name, string deffender, string deffenderEmail);

        Task<bool> SendChallangeRefusedEmail(string mailTo, string name, string deffender);

        Task<bool> SendRegisterConfirmationEmail(string to, string name, string subject, string body);

        Task<bool> SendConfirmResultEmail(string to, string name);
    }
}
