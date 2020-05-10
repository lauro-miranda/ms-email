using LM.Domain.Valuables;
using LM.MSEmail.Messages.Templates;
using LM.RabbitMQ.Messages;

namespace LM.MSEmail.Messages
{
    public class EmailMessage : IEventMessage
    {
        public string TemplateId { get; set; }

        public string Subject { get; set; }

        public string HtmlContent { get; set; }

        public string PlainTextContent { get; set; }

        public ToMessage To { get; set; }

        public FromMessage From { get; set; }

        public TemplateData TemplateData { get; set; }

        public class ToMessage
        {
            public string Name { get; set; }

            public Email Email { get; set; }
        }

        public class FromMessage
        {
            public string Name { get; set; }

            public Email Email { get; set; }
        }
    }
}