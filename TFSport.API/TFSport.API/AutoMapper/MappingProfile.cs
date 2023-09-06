using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using TFSport.API.DTOModels.Users;
using TFSport.Models;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.DTOModels.Users;
using TFSport.Models.Entities;
using TFSport.Services.Interfaces;
using TFSport.Services.Services;

namespace TFSport.API.AutoMapper
{
    public class MappingProfile : Profile
	{
		private readonly UserService _userService;
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

			CreateMap<User, GetAllUsersDTO>();

			CreateMap<User, ChangeUserRoleDTO>();

			CreateMap<User, GetUserByIdDTO>();

			CreateMap<User, UserInfo>();

			CreateMap<GetUserByIdDTO, UserInfo>();

			CreateMap<Article, ArticlesListModel>()
				.ForMember(dest => dest.Author, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src =>src.CreatedTimeUtc));

		}
	}
}
