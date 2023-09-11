using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TFSport.Models.Entities
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
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRoles UserRole { get; set; }

        [JsonIgnore]
        public static string ContainerName { get; set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("verificationToken")]
        public string VerificationToken { get; set; }
    }
}
