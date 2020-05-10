using LM.MSEmail.Messages;
using LM.Responses;
using System.Net;
using System.Threading.Tasks;

namespace LM.MSEmail.Api.Config.Services.Contracts
{
    public interface IEmailService
    {
        Task<Response<HttpStatusCode>> SendMailAsync(EmailMessage message);
    }
}