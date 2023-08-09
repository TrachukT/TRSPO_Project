using Microsoft.Azure.CosmosRepository;
using TFSport.Models;
namespace TFSport.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
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

        public Task<User> RegisterUser(User user)
        {
            return Task.FromResult(user);
        }
    }
}
