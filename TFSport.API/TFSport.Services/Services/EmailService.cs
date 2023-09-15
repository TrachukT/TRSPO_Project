using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using TFSport.Models;
using TFSport.Models.Exceptions;
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
			try
			{
				var link = _emailSettings.EmailUrl + verificationToken;
				var content = $"To complete registration you need to verificate email.To do this click the link below:\n{link}";
				await CreateMessage(email.ToLower(), content, "Email Verification");
				_logger.LogInformation("Email with link for email verification has been send on email {email}", email.ToLower());
			}
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

		public async Task RestorePassword(string email, string verificationToken)
		{
			try
			{
				var link = _emailSettings.PasswordUrl + verificationToken;
				var content = $"To restore password click the link below: \n{link}";
				await CreateMessage(email.ToLower(), content, "Restore Password");
				_logger.LogInformation("Email with link for restoring password has been send on email {email}", email.ToLower());
			}
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
		public async Task CreateMessage(string email, string content,string subject)
		{
			var client = new SendGridClient(_emailSettings.ApiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
				Subject = subject,
				PlainTextContent = content
			};
			msg.AddTo(new EmailAddress(email));
			await client.SendEmailAsync(msg);
		}

        public async Task ArticleIsPublished(string email, string articleName)
        {
			try
			{
				var content = $"Congratulations! Your article \"{articleName}\" was published.";
				await CreateMessage(email.ToLower(), content, "Article is Published");
				_logger.LogInformation("Email about successfull publish of article has been send on email {email}", email.ToLower());
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
        }
    }
}
