using EasyNetQ;
using EasyNetQ.Topology;
using LM.Domain.Helpers;
using LM.MSEmail.Api.Config.Services.Contracts;
using LM.MSEmail.Messages;
using LM.MSEmail.Messages.Config;
using LM.RabbitMQ.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LM.MSEmail.Api.Handles
{
    public class EmailHandle : BackgroundService
    {
        IServiceScopeFactory ScopeFactory { get; }

        public EmailHandle(IServiceScopeFactory scopeFactory)
        {
            ScopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using (var scope = ScopeFactory.CreateScope())
            {
                var serviceBus = scope.ServiceProvider.GetRequiredService<IAdvancedBus>();

                var exchange = serviceBus.ExchangeDeclare(EmailQueueConfig.EmailExchangeName, ExchangeType.Fanout);

                var queue = serviceBus.QueueDeclare("sending.email");

                serviceBus.Bind(exchange, queue, "");

                serviceBus.Consume<EmailMessage>(queue, CreateAsync);
            }

            return Task.CompletedTask;
        }

        async Task CreateAsync(IMessage<EmailMessage> message, MessageReceivedInfo info)
        {
            try
            {
                using (var scope = ScopeFactory.CreateScope())
                {
                    var transactionService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var createResponse = await transactionService.SendMailAsync(message.Body);

                    if (createResponse.HasError)
                        await RiseErrorAsync(string.Join(";", createResponse.Messages.Select(x => x.Text).ToList()), message.Body, ErrorType.BusinessError);
                }
            }
            catch (Exception ex)
            {
                await RiseErrorAsync(ex.ToString(), message.Body, ErrorType.CriticalError);
            }
        }

        async Task RiseErrorAsync(string text, EmailMessage message, ErrorType type = ErrorType.BusinessError)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var serviceBus = scope.ServiceProvider.GetRequiredService<IAdvancedBus>();

                var exchange = serviceBus.ExchangeDeclare(EmailQueueConfig.EmailExchangeName, ExchangeType.Fanout);

                var queue = serviceBus.QueueDeclare("sending.email.error");

                serviceBus.Bind(exchange, queue, "");

                await serviceBus.PublishAsync(exchange, "", false, new Message<ErrorMessage<EmailMessage>>(
                    new ErrorMessage<EmailMessage>(text, DateTimeHelper.GetCurrentDate(), message, type)
                ));
            }
        }
    }
}