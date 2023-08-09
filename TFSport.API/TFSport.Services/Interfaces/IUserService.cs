using TFSport.Models;

namespace TFSport.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> RegisterUser(User user);

        Task<IList<UserRoles>> GetUserRolesByEmailAsync(string email);
    }
}