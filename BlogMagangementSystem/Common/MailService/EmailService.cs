namespace BlogMagangementSystem.Common.EmailService
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendWelcomeEmail(string toemail)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            email.To.Add(MailboxAddress.Parse(toemail));
            email.Subject = "welcome to our platform!";

            var builder = new BodyBuilder
            {
                HtmlBody = $"<h2>hi there!</h2><p>thanks for joining us. we're excited to have you onboard.</p>"
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.DisplayName, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

}
