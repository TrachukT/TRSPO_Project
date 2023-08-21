using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using TFSport.Models;
using TFSport.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TFSport.Services.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> _userRepository;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;
		private readonly ILogger _logger;

		public UserService(IRepository<User> userRepository, IEmailService emailService, IConfiguration configuration, ILogger<UserService> logger)
		{
			_userRepository = userRepository;
			_emailService = emailService;
			_configuration = configuration;
			_logger = logger;
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			var users = await _userRepository.GetAsync(u => u.Email == email);
			return users.FirstOrDefault();
		}

		public async Task<IList<UserRoles>> GetUserRolesByEmailAsync(string email)
		{
			var user = await GetUserByEmailAsync(email);
			if (user != null)
			{
				return new List<UserRoles> { user.UserRole };
			}
			return new List<UserRoles>();
		}

		public async Task<string> ValidateCredentialsAsync(string email, string password)
		{
			var user = await _userRepository.GetAsync(x => x.Email == email).FirstOrDefaultAsync();
			if (user == null)
			{
				throw new ArgumentException(ErrorMessages.InvalidCredentials);
			}

			var passwordHasher = new PasswordHasher();
			var result = passwordHasher.VerifyHashedPassword(user.Password, password);

			if (result != PasswordVerificationResult.Success)
			{
				throw new ArgumentException(ErrorMessages.InvalidCredentials);
			}

			return user.Id;
		}

		public async Task RegisterUser(User user)
		{
			var checkUser = await _userRepository.GetAsync(x => x.Email == user.Email).FirstOrDefaultAsync();
			if (checkUser != null)
				throw new ArgumentException(ErrorMessages.EmailIsRegistered);

			var hash = new PasswordHasher();
			user.Password = hash.HashPassword(user.Password);

			await _userRepository.CreateAsync(user, default);
			await _emailService.EmailVerification(user.Email, user.VerificationToken);
			_logger.LogInformation("User with id {id} was created", user.Id);
		}

		public async Task ForgotPassword(string email)
		{
			var checkUser = await _userRepository.GetAsync(x => x.Email == email).FirstOrDefaultAsync();
			if (checkUser == null)
				throw new ArgumentException(ErrorMessages.NotRegisteredEmail);

			await _emailService.RestorePassword(email, checkUser.VerificationToken);
		}

		public async Task RestorePassword(string token, string password)
		{
			var user = await _userRepository.GetAsync(x => x.VerificationToken == token).FirstOrDefaultAsync();
			if (user == null)
				throw new ArgumentException(ErrorMessages.NotValidLink);

			user.VerificationToken = Guid.NewGuid().ToString();
			var hash = new PasswordHasher();
			user.Password = hash.HashPassword(password);

			await _userRepository.UpdateAsync(user, default);
			_logger.LogInformation("Password for user with id {id} was updated", user.Id);
		}

		public async Task EmailVerification(string verificationToken)
		{
			var user = await _userRepository.GetAsync(x => x.VerificationToken == verificationToken).FirstOrDefaultAsync();
			if (user == null)
				throw new ArgumentException(ErrorMessages.NotValidLink);

			user.VerificationToken = Guid.NewGuid().ToString();
			user.EmailVerified = true;
			await _userRepository.UpdateAsync(user, default);
			_logger.LogInformation("User with id {id} successfully verified email", user.Id);
		}

		public async Task CreateSuperAdminUser()
		{
			var superAdminEmail = _configuration["SuperAdminCredentials:Email"];

			var existingSuperAdmin = await GetUserByEmailAsync(superAdminEmail);
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

		public async Task<List<User>> GetAllUsers()
		{
			var users = await _userRepository.GetAsync(x => true).ToListAsync();
			return users;
		}

		public async Task<bool> ChangeUserRole(string userId, string newUserRole)
		{
			var validRoles = Enum.GetNames(typeof(UserRoles)).Select(role => role.ToLower());
			if (!validRoles.Contains(newUserRole.ToLower()))
			{
				throw new ArgumentException($"Invalid role specified: {newUserRole}.");
			}

			var user = await _userRepository.GetAsync(userId);
			if (user != null)
			{
				user.UserRole = (UserRoles)Enum.Parse(typeof(UserRoles), newUserRole, ignoreCase: true);
				await _userRepository.UpdateAsync(user, default);
				_logger.LogInformation("User with id {id} now has role {role}", user.Id, user.UserRole);
				return true;
			}
			else
			{
				throw new Exception(ErrorMessages.UserNotFound);
			}
		}

		public async Task<User> GetUserById(string id)
		{
			var user = await _userRepository.GetAsync(id);
			if (user == null)
			{
				throw new ArgumentException(ErrorMessages.UserNotFound);
			}
			return user;
		}


	}
}
