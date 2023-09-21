using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IRepository<User> _repository;

        public UsersRepository(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _repository.GetAsync(u => u.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetUsersByFullNameAsync(string fullName)
        {
            var lowerCaseFullName = fullName.ToLower();
            var users = await _repository.GetAsync(u => (u.FirstName + " " + u.LastName).Contains(lowerCaseFullName, StringComparison.OrdinalIgnoreCase)).ToListAsync();

            return users;
        }

        public async Task<User> CheckCredentials(string email,string password)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                throw new CustomException(ErrorMessages.InvalidCredentials);
            }
            return user;
        }

        public async Task<User> ResendEmail(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new CustomException(ErrorMessages.NotRegisteredEmail);
            }

            return user;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _repository.GetAsync(id);
            if (user == null)
            {
                throw new CustomException(ErrorMessages.UserNotFound);
            }
            return user;
        }

        public async Task CreateUser(User user)
        {
            await _repository.CreateAsync(user, default);
        }

        public async Task<List<User>> GetAll()
        {
            var users = await _repository.GetAsync(x => true).ToListAsync();
            return users;
        }

        public async Task<User> FindUserByToken(string token)
        {
            var user = await _repository.GetAsync(x => x.VerificationToken == token).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new CustomException(ErrorMessages.NotValidLink);
            }
            return user;
        }

        public async Task UpdateUser(User user)
        {
            await _repository.UpdateAsync(user, default);
        }

        public async Task<string> ForgotPassword(string email)
        {
            var checkUser = await GetUserByEmail(email);
            if (checkUser == null)
            {
                throw new CustomException(ErrorMessages.NotRegisteredEmail);
            }
            return checkUser.VerificationToken;
        }
    }
}
