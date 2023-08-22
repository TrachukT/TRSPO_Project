using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using TFSport.API;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;
		private readonly ILogger _logger;

		public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
		{
			_emailSettings = emailSettings.Value;
			_logger = logger;
		}

		public async Task EmailVerification(string email, string verificationToken)
		{
			var link = _emailSettings.EmailUrl + verificationToken;
			var content = $"To complete registration you need to verificate email.To do this click the link below:\n{link}";
			await CreateMessage(email, content);
			_logger.LogInformation("Email with link for email verification has been send on email {email}", email);
		}

		public async Task RestorePassword(string email, string verificationToken)
		{
			var link = _emailSettings.PasswordUrl + verificationToken;
			var content = $"To restore password click the link below: \n{link}";
			await CreateMessage(email, content);
			_logger.LogInformation("Email with link for restoring password has been send on email {email}", email);
		}
		public async Task CreateMessage(string email, string content)
		{
			var client = new SendGridClient(_emailSettings.ApiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
				Subject = "Email Verification",
				PlainTextContent = content
			};
			msg.AddTo(new EmailAddress(email));
			await client.SendEmailAsync(msg);
		}
	}
}
