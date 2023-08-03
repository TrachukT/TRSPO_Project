using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using TFSport.Models;
using TFSport.Services.Interfaces;

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

		public async Task RegisterUser(User user)
		{
			try
			{
				var checkUser = await _userRepository.GetAsync(x => x.Email == user.Email).FirstOrDefaultAsync();
				if (checkUser != null)
					throw new Exception(Errors.EmailIsRegistered);
				await _userRepository.CreateAsync(user);
				await _emailService.EmailVerification(user.Email, user.EmailVerificationToken);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
