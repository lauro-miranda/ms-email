using LM.MSEmail.Messages;
using LM.Responses;
using System.Threading.Tasks;

namespace LM.MSEmail.Api.Domain.Services.Contracts
{
    public interface IHistoryService
    {
        Task<Response> SendEmailAsync(EmailMessage message);
    }
}