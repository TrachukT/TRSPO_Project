using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface IUsersRepository
    {
        public Task<User> GetUserByEmail(string email);

        public Task<List<User>> GetUsersByFullNameAsync(string fullName);

        public Task<User> CheckCredentials(string email,string password);

        public Task<User> ResendEmail(string email);

        public Task<User> GetUserById(string id);

        public Task CreateUser(User user);

        public Task<List<User>> GetAll();

        public Task<string> ForgotPassword(string email);

        public Task<User> FindUserByToken(string token);

        public Task UpdateUser(User user);
    }
}
