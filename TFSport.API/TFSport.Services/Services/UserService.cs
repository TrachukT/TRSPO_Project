using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using TFSport.Models;
using TFSport.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace TFSport.Services.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> _userRepository;
		private readonly IEmailService _emailService;

		public UserService(IRepository<User> userRepository, IEmailService emailService)
		{
			_userRepository = userRepository;
			_emailService = emailService;
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

		public async Task RegisterUser(User user)
		{
			var checkUser = await _userRepository.GetAsync(x => x.Email == user.Email).FirstOrDefaultAsync();
			if (checkUser != null)
				throw new Exception(ErrorMessages.EmailIsRegistered);

			var hash = new PasswordHasher();
			user.Password = hash.HashPassword(user.Password);

			await _userRepository.CreateAsync(user, default);
			await _emailService.EmailVerification(user.Email, user.VerificationToken);
		}

		public async Task ForgotPassword(string email)
		{
			var checkUser = await _userRepository.GetAsync(x => x.Email == email).FirstOrDefaultAsync();
			if (checkUser == null)
				throw new Exception(ErrorMessages.NotRegisteredEmail);

			await _emailService.RestorePassword(email, checkUser.VerificationToken);
		}

		public async Task RestorePassword(string token, string password)
		{
			var user = await _userRepository.GetAsync(x => x.VerificationToken == token).FirstOrDefaultAsync();
			if (user == null)
				throw new Exception(ErrorMessages.NotValidLink);

			user.VerificationToken = Guid.NewGuid().ToString();
			var hash = new PasswordHasher();
			user.Password = user.Password = hash.HashPassword(password);

			await _userRepository.UpdateAsync(user, default);
		}

		public async Task EmailVerification(string email, string verificationToken)
		{
			var user = await _userRepository.GetAsync(x => x.VerificationToken == verificationToken).FirstOrDefaultAsync();
			if (user == null)
				throw new Exception(ErrorMessages.NotValidLink);

			if (user.Email != email)
				throw new Exception(ErrorMessages.InvalidCredentials);

			user.VerificationToken = Guid.NewGuid().ToString();
			user.EmailVerified = true;
			await _userRepository.UpdateAsync(user, default);
		}


	}
}
