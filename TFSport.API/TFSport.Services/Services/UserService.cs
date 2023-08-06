using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace TFSport.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosClient _cosmosClient;

        public UserService(IConfiguration configuration, CosmosClient cosmosClient)
        {
            _configuration = configuration;
            _cosmosClient = cosmosClient;
        }

        public async Task<Models.User> GetUserByEmailAsync(string email)
        {
            var container = _cosmosClient.GetContainer(_configuration["CosmosConfiguration:DatabaseId"], "Users");
            var query = $"SELECT * FROM c WHERE c.email = @Email";
            var queryDefinition = new QueryDefinition(query).WithParameter("@Email", email);

            var iterator = container.GetItemQueryIterator<Models.User>(queryDefinition);
            var results = new List<Models.User>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results.FirstOrDefault();
        }

        public Task<Models.User> RegisterUser(Models.User user)
        {
            return Task.FromResult(user);
        }
    }
}
