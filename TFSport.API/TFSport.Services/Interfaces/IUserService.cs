using TFSport.Models.DTOModels.Users;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
    public interface IUserService
	{

        public Task<IList<UserRoles>> GetUserRolesByEmailAsync(string email);

		public Task<string> ValidateCredentialsAsync(string email, string password);

        public Task RegisterUser(UserRegisterDTO user);

		public Task ForgotPassword(string email);

		public Task RestorePassword(string token,string password);

		public Task EmailVerification(string verificationToken);
		
		public Task CreateSuperAdminUser();
		
		public Task<List<GetAllUsersDTO>> GetAllUsers();

        public Task<bool> ChangeUserRole(string userId, UserRoles newUserRole);

		public Task<GetUserByIdDTO> GetUserById(string id);

		public Task ResendEmail(string email);
	}
}
