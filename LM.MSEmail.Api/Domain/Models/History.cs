using LM.Domain.Entities;
using LM.MSEmail.Api.Domain.Valuables;
using LM.MSEmail.Messages;
using LM.Responses;
using LM.Responses.Extensions;
using Newtonsoft.Json;
using System;

namespace LM.MSEmail.Api.Domain.Models
{
    public class History : Entity
    {
        [Obsolete(ConstructorObsoleteMessage, true)]
        public History() { }
        public History(EmailTo to
            , EmailFrom from
            , string templateId
            , string data) : base(Guid.NewGuid())
        {
            To = to;
            From = from;
            TemplateId = templateId;
            TemplateData = data;
        }

        public EmailTo To { get; private set; }

        public EmailFrom From { get; private set; }

        public string TemplateId { get; set; }

        public string TemplateData { get; private set; }

        public string ResponseStatusCode { get; private set; }

        public void SetShippingResponse(string responseStatusCode) => ResponseStatusCode = responseStatusCode;

        public static Response<History> Create(EmailMessage message)
        {
            var response = Response<History>.Create();

            if (message.To == null)
                response.WithBusinessError(nameof(message.To), "Destinatário não informado.");

            if (message.From == null)
                response.WithBusinessError(nameof(message.From), "Remetente não informado.");

            if (response.HasError)
                return response;

            var to = EmailTo.Create(message.To.Name, message.To.Email);

            var from = EmailFrom.Create(message.From.Name, message.From.Email);

            var data = JsonConvert.SerializeObject(message);

            return response.SetValue(new History(to, from, message.TemplateId, data));
        }

        public static implicit operator History(Response<History> response) => response.Data.Value;
    }
}