using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;

namespace TFSport.Models
{
    [PartitionKeyPath("/partitionKey")]
    public class User : BaseModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("userRole")]
        public UserRoles UserRole { get; set; }

        [JsonIgnore]
        public static string ContainerName { get; set; }
    }
}
