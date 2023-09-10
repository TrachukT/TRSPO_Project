using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSport.Services.Interfaces
{
    public interface IEmailService
    {
        public Task EmailVerification(string email, string verificationToken);
        public Task RestorePassword(string email, string EmailVerificationToken);
        public Task ArticleIsPublished(string email, string articleName);
    }
}
