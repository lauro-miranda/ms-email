using LM.Domain.Valuables;
using LM.Responses;
using LM.Responses.Extensions;
using System.Collections.Generic;

namespace LM.MSEmail.Api.Domain.Valuables
{
    public class EmailTo : ValueObject
    {
        EmailTo() { }
        EmailTo(string name, Email email)
        {
            Name = name;
            Email = email.Address;
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Email;
        }

        public static Response<EmailTo> Create(string name, Email email)
        {
            var response = Response<EmailTo>.Create();

            if (string.IsNullOrEmpty(name))
                response.WithBusinessError(nameof(name), "Nome não informado.");

            if (email == null)
                response.WithBusinessError(nameof(email), "Email não informado.");

            if (response.HasError)
                return response;

            return response.SetValue(new EmailTo(name, email));
        }

        public static implicit operator EmailTo(Maybe<EmailTo> entity) => entity.Value;

        public static implicit operator EmailTo(Response<EmailTo> entity) => entity.Data;
    }
}