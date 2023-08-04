using AutoMapper;
using TFSport.API.DTOModels.Users;

namespace TFSport.API
{
	public class AutoUserMapper : Profile
	{
		public AutoUserMapper()
		{
			CreateMap<UserRegisterDTO, Models.User>().BeforeMap((src, dest) =>
			{
				dest.UserRole = Models.UserRoles.User;
				dest.EmailVerified = false;
				dest.VerificationToken = Guid.NewGuid().ToString();
				dest.PartitionKey = dest.Id;
			});
			CreateMap<Models.User, UserRegisterDTO>();
		}
	}
}
