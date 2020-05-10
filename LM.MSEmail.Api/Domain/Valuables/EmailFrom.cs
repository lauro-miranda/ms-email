using LM.Domain.Valuables;
using LM.Responses;
using LM.Responses.Extensions;
using System.Collections.Generic;

namespace LM.MSEmail.Api.Domain.Valuables
{
    public class EmailFrom : ValueObject
    {
        EmailFrom() { }
        EmailFrom(string name, Email email)
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

        public static Response<EmailFrom> Create(string name, Email email)
        {
            var response = Response<EmailFrom>.Create();

            if (string.IsNullOrEmpty(name))
                response.WithBusinessError(nameof(name), "Nome não informado.");

            if (email == null)
                response.WithBusinessError(nameof(email), "Email não informado.");

            if (response.HasError)
                return response;

            return response.SetValue(new EmailFrom(name, email));
        }

        public static implicit operator EmailFrom(Maybe<EmailFrom> entity) => entity.Value;

        public static implicit operator EmailFrom(Response<EmailFrom> entity) => entity.Data;
    }
}