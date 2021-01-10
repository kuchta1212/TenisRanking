using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SendGrid;
using SendGrid.Helpers.Mail;
using TenisRanking.Job;
using TenisRanking.Models;
using TenisRanking.Resources;

namespace TenisRanking.Email
{
    public class EmailController : IEmailController
    {
        private readonly IStringLocalizer<Resource> localizer;
        private readonly IOptions<EmailOptions> options;
        private readonly IOptions<MatchDaysLimitOptions> matchDaysLimitOptions;

        public EmailController(IStringLocalizer<Resource> localizer, IOptions<EmailOptions> options, IOptions<MatchDaysLimitOptions> matchDaysLimitOptions)
        {
            this.localizer = localizer;
            this.options = options;
            this.matchDaysLimitOptions = matchDaysLimitOptions;
        }

        public Task<bool> SendChallangeEmail(Player deffender, Player challanger, string matchId)
        {
            var emailBodyFormat = this.localizer[Messages.ChallangeEmail].Value;
            var emailBody = string.Format(emailBodyFormat, challanger.PlayerName, deffender.Rank, challanger.Rank, this.GetDaysLimit(deffender.LastPlayedMatch), challanger.UserName, matchId);
            return this.SendEmail(deffender.UserName, deffender.PlayerName, emailBody, "Svoboda tenis žebříček - Výzva k zápasu");
        }

        public Task<bool> SendChallangeAcceptedEmail(Player deffender, Player challanger)
        {
            var emailFormat = this.localizer[Messages.ChallangeAcceptedEmail].Value;
            var email = string.Format(emailFormat, challanger, challanger.Rank, deffender.Rank, this.GetDaysLimit(challanger.LastPlayedMatch), deffender.UserName);
            return this.SendEmail(challanger.UserName, challanger.PlayerName, email, "Svoboda tenis žebříček - Výzva přijmuta");
        }

        public Task<bool> SendChallangeRefusedEmail(string mailTo, string name, Player deffender, string matchId)
        {
            var emailFormat = this.localizer[Messages.ChallangeRefusedEmail].Value;
            var email = string.Format(emailFormat, deffender.PlayerName, matchId);
            return this.SendEmail(mailTo, name, email, "Svoboda tenis žebříček - Výzva odmítnuta");
        }

        public Task<bool> SendRegisterConfirmationEmail(string to, string name, string subject, string link)
        {
            var emailFormat = this.localizer[Messages.RegisterEmail].Value;
            var email = string.Format(emailFormat, link);
            return this.SendEmail(to, name, email, subject);
        }

        public Task<bool> SendConfirmResultEmail(Player receiver, Player opponent, Match match, bool isReceiverDeffender)
        {
            var emailFormat = this.localizer[Messages.ConfirmResultEmail].Value;
            var email = string.Format(
                emailFormat, 
                opponent.PlayerName, 
                match.Id, 
                receiver.Rank, 
                opponent.Rank, 
                isReceiverDeffender 
                    ? receiver.PlayerName 
                    : opponent.PlayerName, 
                isReceiverDeffender
                    ? opponent.PlayerName
                    : receiver.PlayerName,
                match.Result);
            return this.SendEmail(receiver.UserName, receiver.PlayerName, email, "Svoboda tenis žebříček - Výsledek čeká na potvrzení");
        }

        private int GetDaysLimit(DateTime lastGamePlayed) => this.matchDaysLimitOptions.Value.Days - (DateTime.Now.Date - lastGamePlayed).Days;

        private async Task<bool> SendEmail(string mailTo, string name, string body, string subject)
        {
            if (!this.options.Value.Enabled)
            {
                return true;
            }

            var emailFormat = this.localizer[Messages.EmailFormat].Value;
            var emailBody = string.Format(emailFormat, body);

            var client = new SendGridClient(this.options.Value.SendGridKeyApi);
            var from = new EmailAddress(this.options.Value.SenderEmail, this.options.Value.SenderName);
            var to = new EmailAddress(mailTo, name);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, emailBody);
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                return false;
            }

            return true;
        }
    }
}
