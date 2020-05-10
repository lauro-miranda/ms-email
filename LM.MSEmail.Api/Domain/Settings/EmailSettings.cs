namespace LM.MSEmail.Api.Domain.Settings
{
    public class EmailSettings
    {
        public SendGridSettings SendGrid { get; set; }

        public class SendGridSettings
        {
            public string Key { get; set; }
        }
    }
}