using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using TFSport.API.DTOModels.Articles;
using TFSport.API.DTOModels.Users;
using TFSport.Models;
using TFSport.Services.Interfaces;
using TFSport.Services.Services;

namespace TFSport.API.AutoMapper
{
	public class MappingProfile : Profile
	{
		private readonly IUserService _userService;
		public MappingProfile(IUserService userService)
		{
			_userService = userService;
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

			CreateMap<User, UserDTO>();

			//CreateMap<Article, GetArticlesListDTO>()
			//	.ForMember(dest => dest.Author, opt => opt.MapFrom(src => _userService.GetUserById(src.Author)));

			//CreateMap<List<Article>, List<GetArticlesListDTO>>()
			//	.BeforeMap(async (src, dest) =>
			//	{
			//		foreach (var articleDto in dest)
			//		{
			//			var user = await _userService.GetUserById(articleDto.Author.Id);

			//			var userDto = new UserDTO
			//			{
			//				Id = user.Id,
			//				FirstName = user.FirstName,
			//				LastName = user.LastName,
			//			};

			//			articleDto.Author = userDto;
			//		}
			//	});
			CreateMap<User, UserDTO>(); // Ensure User to UserDTO mapping is configured
										//CreateMap<List<Article>, List<GetArticlesListDTO>>();

			//CreateMap<Article, GetArticlesListDTO>()
			//	.BeforeMap(async (src, dest, context) =>
			//	{
			//		// Map the list of articles first
			//		context.Mapper.Map(src, dest);

			//		// Perform additional mapping for the Author property
			//		var user = await context.Mapper.Map<Task<UserDTO>>(_userService.GetUserById(src.Author));
			//		dest.Author = user;
			//	});

			CreateMap<Article, GetArticlesListDTO>()
			.ForMember(dest => dest.Author, opt => opt.MapFrom((src, dest, destMember, context) =>
			{
				var user = _userService.GetUserById(src.Author).Result; // Make sure to handle async properly
				return context.Mapper.Map<UserDTO>(user);
			}));
		}
	}
}
