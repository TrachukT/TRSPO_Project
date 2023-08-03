using System;
using System.Collections.Generic;
using System.Linq;
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

        }
    }
}
