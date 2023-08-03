namespace TFSport.Services
{
    public class UserService
    {
        //Empty method for user registration
        public Task<Models.User> RegisterUser(Models.User user)
        {
            return Task.FromResult(user);
        }
    }
}
