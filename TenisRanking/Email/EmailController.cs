using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TenisRanking.Email
{
    public class EmailController : IEmailController
    {
        private readonly IOptions<EmailOptions> options;

        public EmailController(IOptions<EmailOptions> options)
        {
            this.options = options;
        }


        private async Task<bool> SendEmail(string mailTo, string name, string body, string subject)
        {
            var client = new SendGridClient(this.options.Value.SendGridKeyApi);
            var from = new EmailAddress(this.options.Value.SenderEmail, this.options.Value.SenderName);
            var to = new EmailAddress(mailTo, name);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, body);
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                return false;
            }

            return true;
        }

        public Task<bool> SendChallangeEmail()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendChallangeAcceptedEmail()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendChallangeRefusedEmail()
        {
            throw new NotImplementedException();
        }
    }
}
