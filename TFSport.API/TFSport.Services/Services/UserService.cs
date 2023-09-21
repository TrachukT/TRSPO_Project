using TFSport.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using AutoMapper;
using TFSport.Models.DTOModels.Users;

namespace TFSport.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserService(IEmailService emailService, IConfiguration configuration, ILogger<UserService> logger, IUsersRepository usersRepository,IMapper mapper)
        {
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _usersRepository = usersRepository;
            _mapper = mapper;
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
                var user = await _usersRepository.CheckCredentials(email, password);
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
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task RegisterUser(UserRegisterDTO model)
        {
            try
            {
                var checkuser = await _usersRepository.GetUserByEmail(model.Email);
                if (checkuser != null)
                {
                    throw new CustomException(ErrorMessages.EmailIsRegistered);
                }
                var user = _mapper.Map<User>(model);
                var hash = new PasswordHasher();
                user.Password = hash.HashPassword(user.Password);
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
                var user = await _usersRepository.FindUserByToken(verificationToken);
                user.VerificationToken = Guid.NewGuid().ToString();
                user.EmailVerified = true;
                await _usersRepository.UpdateUser(user);
                _logger.LogInformation("User with id {id} successfully verified email", user.Id);
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
                        VerificationToken = Guid.NewGuid().ToString(),
                    };

                    superAdmin.PartitionKey = superAdmin.Id;
                    await _usersRepository.CreateUser(superAdmin);
                    await _emailService.EmailVerification(superAdmin.Email, superAdmin.VerificationToken);
                    _logger.LogInformation("Super admin created");
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<GetAllUsersDTO>> GetAllUsers()
        {
            try
            {
                var users = await _usersRepository.GetAll();
                List<GetAllUsersDTO> list = _mapper.Map<List<GetAllUsersDTO>>(users);
                return list;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<bool> ChangeUserRole(string userId, UserRoles newUserRole)
        {
            try
            {
                var validRoles = Enum.GetValues(typeof(UserRoles)).Cast<UserRoles>().ToList();

                if (!validRoles.Contains(newUserRole))
                {
                    throw new CustomException($"Invalid role specified: {newUserRole}.");
                }

                var user = await _usersRepository.GetUserById(userId);
                user.UserRole = newUserRole;
                await _usersRepository.UpdateUser(user);
                _logger.LogInformation("User with id {id} now has role {role}", user.Id, user.UserRole);
                return true;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<GetUserByIdDTO> GetUserById(string id)
        {
            try
            {
                var user = await _usersRepository.GetUserById(id);
                return _mapper.Map<GetUserByIdDTO>(user);
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
                if (user.EmailVerified == true)
                {
                    throw new CustomException(ErrorMessages.AlreadyVerifiedEmail);
                }

                await _emailService.EmailVerification(email, user.VerificationToken);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
