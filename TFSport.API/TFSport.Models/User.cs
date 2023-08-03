using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;

namespace TFSport.Models
{
    [PartitionKeyPath("/partitionKey")]
    public class User : BaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

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
