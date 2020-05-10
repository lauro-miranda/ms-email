using LM.MSEmail.Api.Config.Services.Contracts;
using LM.MSEmail.Api.Domain.Settings;
using LM.MSEmail.Messages;
using LM.Responses;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

namespace LM.MSEmail.Api.Config.Services
{
    public class EmailService : IEmailService
    {
        SendGridClient SendGridClient { get; }

        public EmailService(IOptions<EmailSettings> options)
        {
            SendGridClient = new SendGridClient(options.Value.SendGrid.Key);
        }

        public async Task<Response<HttpStatusCode>> SendMailAsync(EmailMessage message)
        {
            var emailMessage = MailHelper.CreateSingleEmail(
                    new EmailAddress(message.From.Email.Address, message.From.Name),
                    new EmailAddress(message.To.Email.Address, message.To.Name),
                    message.Subject,
                    message.PlainTextContent,
                    message.HtmlContent);

            if (!string.IsNullOrEmpty(message.TemplateId))
                emailMessage.TemplateId = message.TemplateId;

            if (message.TemplateData != null)
                emailMessage.SetTemplateData(message.TemplateData);

            var response = await SendGridClient.SendEmailAsync(emailMessage);

            return Response<HttpStatusCode>.Create(response.StatusCode);
        }
    }
}