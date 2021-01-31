
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace PowerUp.Mvc.Security
{
    public class EmailIdentityService: SendGrid.ISendGridClient
    {
        public string UrlPath { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Version { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string MediaType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public AuthenticationHeaderValue AddAuthorization(KeyValuePair<string, string> header)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response> MakeRequest(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response> RequestAsync(BaseClient.Method method, string requestBody = null, string queryParams = null, string urlPath = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Response> SendEmailAsync(SendGridMessage message, CancellationToken cancellationToken = default)
        {
            var sendGridSender = ConfigurationManager.AppSettings["SendGridMailFrom"].ToString();
            message.From = new EmailAddress(sendGridSender);

            var sendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"].ToString();
            return await new SendGridClient(sendGridApiKey)
                .SendEmailAsync(message, cancellationToken);
        }
    }
}
