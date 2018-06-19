using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.Mvc.Security
{
    public class EmailIdentityService: IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"].ToString();
            var sendGridSender = ConfigurationManager.AppSettings["SendGridMailFrom"].ToString();
            dynamic sg = new SendGridAPIClient(sendGridApiKey);
            var body = new Content("text/plain", message.Body);
            var mail = new Mail(new Email(sendGridSender), message.Subject, new Email(message.Destination), body);
            dynamic response = sg.client.mail.send.post(requestBody: mail.Get());
            
            return Task.FromResult(0);
        }
    }
}
