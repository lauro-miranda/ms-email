using LM.Domain.UnitOfWork;
using LM.MSEmail.Api.Config.Services.Contracts;
using LM.MSEmail.Api.Domain.Models;
using LM.MSEmail.Api.Domain.Repositories;
using LM.MSEmail.Api.Domain.Services.Contracts;
using LM.MSEmail.Messages;
using LM.Responses;
using LM.Responses.Extensions;
using System;
using System.Threading.Tasks;

namespace LM.MSEmail.Api.Domain.Services
{
    public class HistoryService : IHistoryService
    {
        IHistoryRepository HistoryRepository { get; }

        IUnitOfWork Uow { get; }

        IEmailService EmailService { get; }

        public HistoryService(IHistoryRepository historyRepository
            , IUnitOfWork uow
            , IEmailService emailService)
        {
            HistoryRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
            Uow = uow ?? throw new ArgumentNullException(nameof(Uow));
            EmailService = emailService ?? throw new ArgumentNullException(nameof(EmailService));
        }

        public async Task<Response> SendEmailAsync(EmailMessage message)
        {
            var response = Response.Create();

            var history = History.Create(message);

            if (history.HasError)
                return response.WithMessages(history.Messages);

            var responseStatusCode = await EmailService.SendMailAsync(message);

            if (responseStatusCode.HasError)
                response.WithMessages(responseStatusCode.Messages);

            history.Data.Value.SetShippingResponse($"{responseStatusCode.Data.Value}");

            await HistoryRepository.AddAsync(history);

            if (!await Uow.CommitAsync())
                return response.WithCriticalError("Falha ao tentar gravar o histório.");

            return response;
        }

    }
}