using TFSport.Models;

namespace TFSport.Services.Interfaces
{
	public interface IUserService
	{
        public Task<User> GetUserByEmailAsync(string email);

        public Task<IList<UserRoles>> GetUserRolesByEmailAsync(string email);

		public Task<string> ValidateCredentialsAsync(string email, string password);

        public Task RegisterUser(User user);

		public Task ForgotPassword(string email);

		public Task RestorePassword(string token,string password);

		public Task EmailVerification(string verificationToken);
		
		public Task CreateSuperAdminUser();
		
		public Task<List<User>> GetAllUsers();
        public Task<bool> ChangeUserRole(string userId, string newUserRole);
		public Task<User> GetUserById(string id);

	}
}
