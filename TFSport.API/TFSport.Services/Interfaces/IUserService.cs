using TFSport.Models;

namespace TFSport.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<RegistrationModel> RegisterUser(RegistrationModel user);
    }
}