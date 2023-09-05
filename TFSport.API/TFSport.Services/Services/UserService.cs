﻿using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using TFSport.Models;
using TFSport.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;

namespace TFSport.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger _logger;

        public UserService(IEmailService emailService, IConfiguration configuration, ILogger<UserService> logger, IUsersRepository usersRepository)
        {
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _usersRepository = usersRepository;
        }

        public async Task<IList<UserRoles>> GetUserRolesByEmailAsync(string email)
        {
            try
            {
                var user = await _usersRepository.GetUserByEmail(email);
                if (user != null)
                {
                    return new List<UserRoles> { user.UserRole };
                }
                return new List<UserRoles>();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<string> ValidateCredentialsAsync(string email, string password)
        {
            try
            {
                var userId = await _usersRepository.CheckCredentials(email, password);
                return userId;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task RegisterUser(User user)
        {
            try
            {
                await _usersRepository.CreateUser(user);
                await _emailService.EmailVerification(user.Email, user.VerificationToken);
                _logger.LogInformation("User with id {id} was created", user.Id);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }

        }

        public async Task ForgotPassword(string email)
        {
            try
            {
                var verificationToken = await _usersRepository.ForgotPassword(email);
                await _emailService.RestorePassword(email, verificationToken);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task RestorePassword(string token, string password)
        {
            try
            {
                var user = await _usersRepository.FindUserByToken(token);
                user.VerificationToken = Guid.NewGuid().ToString();
                var hash = new PasswordHasher();
                user.Password = hash.HashPassword(password);

                await _usersRepository.UpdateUser(user);
                _logger.LogInformation("Password for user with id {id} was updated", user.Id);

            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task EmailVerification(string verificationToken)
        {
            try
            {
                var userId = await _usersRepository.EmailVerification(verificationToken);
                _logger.LogInformation("User with id {id} successfully verified email", userId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task CreateSuperAdminUser()
        {
            try
            {
                var superAdminEmail = _configuration["SuperAdminCredentials:Email"];

                var existingSuperAdmin = await _usersRepository.GetUserByEmail(superAdminEmail);
                if (existingSuperAdmin == null)
                {
                    var superAdmin = new User
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        Email = superAdminEmail,
                        Password = _configuration["SuperAdminCredentials:Password"],
                        UserRole = UserRoles.SuperAdmin,
                        EmailVerified = true,
                        VerificationToken = Guid.NewGuid().ToString()
                    };

                    superAdmin.PartitionKey = superAdmin.Id;
                    await RegisterUser(superAdmin);
                    _logger.LogInformation("Super admin created");
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var users = await _usersRepository.GetAll();
                return users;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<bool> ChangeUserRole(string userId, string newUserRole)
        {
            try
            {
                var validRoles = Enum.GetNames(typeof(UserRoles)).Select(role => role.ToLower());
                if (!validRoles.Contains(newUserRole.ToLower()))
                {
                    throw new CustomException($"Invalid role specified: {newUserRole}.");
                }

                var user = await _usersRepository.GetUserById(userId);
                user.UserRole = (UserRoles)Enum.Parse(typeof(UserRoles), newUserRole, ignoreCase: true);
                await _usersRepository.UpdateUser(user);
                _logger.LogInformation("User with id {id} now has role {role}", user.Id, user.UserRole);
                return true;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<User> GetUserById(string id)
        {
            try
            {
                var user = await _usersRepository.GetUserById(id);
                return user;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task ResendEmail(string email)
        {
            try
            {
                var user = await _usersRepository.ResendEmail(email);

                await _emailService.EmailVerification(email, user.VerificationToken);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

    }
}
