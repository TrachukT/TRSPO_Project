using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class EmailService:IEmailService
    {
        public EmailService()
        {

        }
        public async Task EmailVerification(string email, string verificationToken)
        {
			var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
			var client = new SendGridClient(apiKey);
			var verificationLink = "";
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("[REPLACE WITH YOUR EMAIL]", "[REPLACE WITH YOUR NAME]"),
				Subject = "Email Verification",
				PlainTextContent = $"To complete registration you need to verificate email.To do this click the link below:\n{verificationLink}"
			};
			msg.AddTo(new EmailAddress(email, "TFSport"));
			var response = await client.SendEmailAsync(msg);
		}
    }
}
