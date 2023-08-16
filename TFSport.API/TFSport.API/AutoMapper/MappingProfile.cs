using AutoMapper;
using TFSport.API.DTOModels.Users;
using TFSport.Models;

namespace TFSport.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDTO, User>().BeforeMap((src, dest) =>
            {
                dest.UserRole = UserRoles.User;
                dest.EmailVerified = false;
                dest.VerificationToken = Guid.NewGuid().ToString();
                dest.PartitionKey = dest.Id; 
            });

            CreateMap<User, UserRegisterDTO>();

            CreateMap<User, UserLoginDTO>();

            CreateMap<User, ChangeUserRoleDTO>();
        }
    }
}
