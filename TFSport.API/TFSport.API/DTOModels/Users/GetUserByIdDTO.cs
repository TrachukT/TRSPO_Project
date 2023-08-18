using TFSport.Models;

namespace TFSport.API.DTOModels.Users
{
	public class GetUserByIdDTO
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
        public UserRoles UserRole { get; set; }
	}
}
