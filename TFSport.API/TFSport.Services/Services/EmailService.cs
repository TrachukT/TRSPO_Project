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
			var client = new SendGridClient(_emailSettings.ApiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(_emailSettings.SenderEmail,_emailSettings.SenderName),
				Subject = "Email Verification",
				PlainTextContent = $"To complete registration you need to verificate email.To do this click the link below:\n{_emailSettings.EmailUrl}"
			};
			msg.AddTo(new EmailAddress(email));
			await client.SendEmailAsync(msg);
		}
		
		public async Task RestorePassword(string email, string verificationToken)
		{
			var client = new SendGridClient(_emailSettings.ApiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(_emailSettings.SenderEmail,_emailSettings.SenderName),
				Subject = "Restore Password",
				PlainTextContent = $"To restore password click the link below: \n{_emailSettings.PasswordUrl}"
			};
			msg.AddTo(new EmailAddress(email));
			await client.SendEmailAsync(msg);
		}
	}
}
