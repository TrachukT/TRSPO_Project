using AutoMapper;
using TFSport.API.DTOModels.Articles;
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

			CreateMap<User, GetAllUsersDTO>();

			CreateMap<User, ChangeUserRoleDTO>();

			CreateMap<User, GetUserByIdDTO>();

			CreateMap<User, UserInfo>();

			CreateMap<Article, ArticlesListModel>()
				.ForMember(dest => dest.Author, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src =>src.CreatedTimeUtc));

            CreateMap<ArticleCreateDTO, Article>().BeforeMap((src, dest) =>
            {
                dest.PartitionKey = dest.Id;
            });

			CreateMap<Article, ArticleCreateDTO>();

            CreateMap<Article, GetArticleWithContentDTO>();

			CreateMap<Article, ArticleUpdateDTO>().ReverseMap();
        }
    }
}
