using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Email
{
    public interface IEmailController
    {
        Task<bool> SendChallangeEmail(string mailTo, string name, string challanger);

        Task<bool> SendChallangeAcceptedEmail(string mailTo, string name, string deffender);

        Task<bool> SendChallangeRefusedEmail(string mailTo, string name, string deffender);

        Task<bool> SendRegisterConfirmationEmail(string to, string name, string subject, string body);
    }
}
