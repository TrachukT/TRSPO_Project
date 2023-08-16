using TFSport.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace TFSport.API.DTOModels.Users
{
	public class GetAllUsersDTO
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool EmailVerified { get; set; }
		[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
		[JsonPropertyName("UserRole")]
		public UserRoles UserRole { get; set; }
	}
}
