using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
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
            if (!this.options.Value.Enabled)
            {
                return true;
            }

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

        public Task<bool> SendChallangeEmail(string mailTo, string name, string challanger)
        {
            var email = $"<h1>Byl si vyzván!</h1><p>Byl si vyzván na zápas.</p></br><p><b>Vyzivatel:</b> {challanger}</p></br></br><p>Prosím reaguj na tuto výzvu na webu: <a href=\"https://tenis-svoboda.azurewebsites.net\">Žebříček Svoboda</a></p>";
            return this.SendEmail(mailTo, name, email, "Svoboda tenis žebříček - Výzva k zápasu");
        }

        public Task<bool> SendChallangeAcceptedEmail(string mailTo, string name, string deffender)
        {
            var email = $"<h1>Výzva byla přijata!</h1><p>Tvoje výzva byla přijata.</p></br><p><b>Vyzivatel:</b> {name}</p></br><p><b>Obhájce:</b> {deffender}</p></br></br><p>Nezapomeňte zadat výsledek pak na web: <a href=\"https://tenis-svoboda.azurewebsites.net\">Žebříček Svoboda</a></p>";
            return this.SendEmail(mailTo, name, email, "Svoboda tenis žebříček - Výzva přijmuta");
        }

        public Task<bool> SendChallangeRefusedEmail(string mailTo, string name, string deffender)
        {
            var email = $"<h1>Výzva byla odmítnuta!</h1><p>Tvoje výzva byla odmítnuta.</p></br><p><b>Obhájce:</b> {deffender}</p></br></br><p>Zkus někoho jiného.</p><p><a href=\"https://tenis-svoboda.azurewebsites.net\">Žebříček Svoboda</a></p>";
            return this.SendEmail(mailTo, name, email, "Svoboda tenis žebříček - Výzva odmítnuta");
        }

        public Task<bool> SendRegisterConfirmationEmail(string to, string name, string subject, string body) => this.SendEmail(to, name, body, subject);

        public Task<bool> SendConfirmResultEmail(string to, string name)
        {
            var email = $"<h1>Výsledek čeká na potvrzení!</h1><p>Výsledek z tvého posledního zápasu čeká na potvrzení. Od této chvíle máte 48h na kontrolu a potvrzení nebo opravení. </p></br></br><p><a href=\"https://tenis-svoboda.azurewebsites.net\">Žebříček Svoboda</a></p>";
            return this.SendEmail(to, name, email, "Svoboda tenis žebříček - Výsledek čeká na potvrzení");
        }
    }
}
