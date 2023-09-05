using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface IUsersRepository
    {
        public Task<User> GetUserByEmail(string email);
        public Task<string> CheckCredentials(string email,string password);
        public Task<User> ResendEmail(string email);
        public Task<User> GetUserById(string id);
        public Task CreateUser(User user);
        public Task<List<User>> GetAll();
        public Task<string> EmailVerification(string verificationToken);
        public Task<string> ForgotPassword(string email);
        public Task<User> FindUserByToken(string token);
        public Task UpdateUser(User user);
    }
}
