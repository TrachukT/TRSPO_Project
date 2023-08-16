using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TFSport.API;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class EmailService:IEmailService
    {
		private readonly EmailSettings _emailSettings;

		public EmailService(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}

		public async Task EmailVerification(string email, string verificationToken)
        {
			var content = $"To complete registration you need to verificate email.To do this click the link below:\n{_emailSettings.EmailUrl}";
			await CreateMessage(email, content);
		}

		public async Task RestorePassword(string email, string verificationToken)
		{
			var content = $"To restore password click the link below: \n{_emailSettings.PasswordUrl}";
			await CreateMessage(email, content);
		}
		public async Task CreateMessage(string email,string content)
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
