﻿using AutoMapper;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.DTOModels.Users;
using TFSport.Models.Entities;

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
				dest.Favorites = new List<string>();
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

            CreateMap<ArticleCreateDTO, Article>().BeforeMap((src, dest) =>
            {
                dest.PartitionKey = dest.Id;
                dest.UpdatedAt = DateTime.UtcNow;
                dest.Status = ArticleStatus.Draft;
            });

			CreateMap<Article, ArticleCreateDTO>();

			CreateMap<Article, ArticleWithContentDTO>()
				.ForMember(dest => dest.Author, opt => opt.Ignore())
				.ForMember(dest => dest.Content, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedTimeUtc));

            CreateMap<ArticleUpdateDTO, Article>().BeforeMap((src, dest) =>
			{
				dest.UpdatedAt = DateTime.UtcNow;
				dest.Status = ArticleStatus.Draft;
			});

            CreateMap<Article, ArticleUpdateDTO>().ReverseMap();
        }
    }
}
