using Microsoft.AspNet.Identity;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<string> CheckCredentials(string email,string password)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                throw new CustomException(ErrorMessages.InvalidCredentials);
            }

            if (user.EmailVerified == false)
                throw new CustomException(ErrorMessages.EmailNotVerified);

            var passwordHasher = new PasswordHasher();
            var result = passwordHasher.VerifyHashedPassword(user.Password, password);

            if (result != PasswordVerificationResult.Success)
            {
                throw new CustomException(ErrorMessages.InvalidCredentials);
            }
            return user.Id;
        }

        public async Task<User> ResendEmail(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new CustomException(ErrorMessages.NotRegisteredEmail);
            }

            if (user.EmailVerified == true)
            {
                throw new CustomException(ErrorMessages.AlreadyVerifiedEmail);
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
            var checkUser = await GetUserByEmail(user.Email);
            if (checkUser != null)
            {
                throw new CustomException(ErrorMessages.EmailIsRegistered);
            }

            var hash = new PasswordHasher();
            user.Password = hash.HashPassword(user.Password);

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

        public async Task<string> EmailVerification(string verificationToken)
        {
            var user = await FindUserByToken(verificationToken);

            user.VerificationToken = Guid.NewGuid().ToString();
            user.EmailVerified = true;
            await UpdateUser(user);
            return user.Id;
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
