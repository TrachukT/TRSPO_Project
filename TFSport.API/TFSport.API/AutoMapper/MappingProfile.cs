using AutoMapper;
using TFSport.API.DTOModels.Users;

namespace TFSport.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
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
