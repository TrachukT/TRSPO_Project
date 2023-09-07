using TFSport.Models.Entities;

namespace TFSport.Models.DTOModels.Users
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
