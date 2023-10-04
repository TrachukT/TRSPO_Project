using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using TFSport.Models;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.DTOModels.Users;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Repository.Repositories;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticlesRepository _articleRepository;
        private readonly IUsersRepository _userRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IAuthorStatisticsRepository _authorStatisticsRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IFavoritesService _favoritesService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly BlobStorageOptions _blobOptions;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly ITagsService _tagsService;

        public ArticleService(IOptions<BlobStorageOptions> blobOptions, IArticlesRepository articleRepository, IUsersRepository userRepository,
            IUserService userService, IMapper mapper, IBlobStorageService blobStorageService, ILogger<ArticleService> logger,
            IEmailService emailService, ITagsService tagsService, ITagsRepository tagsRepository, IFavoritesService favoritesService, 
            IAuthorStatisticsRepository authorStatisticsRepository, ICommentsRepository commentsRepository)
        {
            _articleRepository = articleRepository;
            _userService = userService;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _blobOptions = blobOptions.Value;
            _logger = logger;
            _userRepository = userRepository;
            _emailService = emailService;
            _tagsService = tagsService;
            _tagsRepository = tagsRepository;
            _favoritesService = favoritesService;
            _authorStatisticsRepository = authorStatisticsRepository;
            _commentsRepository = commentsRepository;
        }

        public async Task<OrderedArticlesDTO> ArticlesForApprove(int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = x => x.Status == ArticleStatus.Review;
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy,predicate);
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = await _articleRepository.GetCountofArticles(predicate),
                    Articles = await MapArticles(articles.ToList())
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> AuthorsArticles(int pageNumber, int pageSize, string orderBy,string authorId)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = x => x.Author == authorId;
                var articles = await _articleRepository.GetArticles(pageNumber,pageSize,orderBy,predicate);
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = await _articleRepository.GetCountofArticles(predicate),
                    Articles = await MapArticles(articles.ToList())
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> PublishedArticles(int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = article => article.Status == ArticleStatus.Published;
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, predicate);
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = await _articleRepository.GetCountofArticles(predicate),
                    Articles = await MapArticles(articles.ToList())
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> GetArticlesByTagAsync(string tagName, int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                var tag = await _tagsRepository.GetTagAsync(tagName);
                if (tag != null)
                {
                    var articleIds = tag.ArticleIds;
                    var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, articleIds: articleIds);

                    return new OrderedArticlesDTO
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalCount = await _articleRepository.GetCountofArticles(article => articleIds.Contains(article.Id)),
                        Articles = await MapArticles(articles.ToList())
                    };
                }
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = 0,
                    Articles = new List<ArticlesListModel>()
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> SearchArticlesByTagsAsync(string substring, int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = article => article.Tags.Any(tag => tag.Contains(substring, StringComparison.OrdinalIgnoreCase)) && article.Status == ArticleStatus.Published;
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, predicate);

                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Articles = await MapArticles(articles.ToList()),
                    TotalCount = await _articleRepository.GetCountofArticles(predicate)
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> SearchArticlesByTitleAsync(string substring, int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = article => article.Title.Contains(substring, StringComparison.OrdinalIgnoreCase) && article.Status == ArticleStatus.Published;
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, predicate);

                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Articles = await MapArticles(articles.ToList()),
                    TotalCount = await _articleRepository.GetCountofArticles(predicate)
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> SearchArticlesByAuthorNameAsync(string authorName, int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                var users = await _userRepository.GetUsersByFullNameAsync(authorName);
                var userIds = users.Select(user => user.Id).ToList();

                Expression<Func<Article, bool>> predicate = article =>
                    userIds.Contains(article.Author) && article.Status == ArticleStatus.Published;

                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, predicate);

                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Articles = await MapArticles(articles.ToList()),
                    TotalCount = await _articleRepository.GetCountofArticles(predicate)
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<ArticleWithContentDTO> GetArticleWithContentByIdAsync(string articleId)
        {
            try
            {
                var article = await _articleRepository.GetArticleByIdAsync(articleId);
                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                var articleWithContentDTO = await MapArticleWithContentAsync(article);
                return articleWithContentDTO;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<ArticlesListModel>> MapArticles(List<Article> articles)
        {
            if (articles.Count == 0)
            {
                return new List<ArticlesListModel>();
            }
            var list = new List<ArticlesListModel>();
            foreach (var article in articles)
            {
                var user = await _userService.GetUserById(article.Author);
                var userDTO = _mapper.Map<UserInfo>(user);
                var articleDTO = _mapper.Map<ArticlesListModel>(article);
                articleDTO.Author = userDTO;
                list.Add(articleDTO);
            }
            return list;
        }

        public async Task<ArticleWithContentDTO> MapArticleWithContentAsync(Article article)
        {
            var content = await _blobStorageService.GetHtmlContentAsync(_blobOptions.ArticleContainer, article.Id);
            var user = await _userService.GetUserById(article.Author);
            var userDTO = _mapper.Map<UserInfo>(user);

            var articleWithContentDTO = _mapper.Map<ArticleWithContentDTO>(article);
            articleWithContentDTO.Content = content;
            articleWithContentDTO.Author = userDTO;

            return articleWithContentDTO;
        }

        public async Task CreateArticleAsync(ArticleCreateDTO articleDTO)
        {
            try
            {
                var existingArticle = await _articleRepository.GetArticleByTitleAsync(articleDTO.Title);

                if (existingArticle != null)
                {
                    throw new CustomException(ErrorMessages.ArticleWithThisTitleExists);
                }

                var existingUser = await _userRepository.GetUserById(articleDTO.Author);

                if (existingUser == null)
                {
                    throw new CustomException(ErrorMessages.UserNotFound);
                }

                var article = _mapper.Map<Article>(articleDTO);

                article.Status = ArticleStatus.Review;

                await _blobStorageService.UploadHtmlContentAsync(_blobOptions.ArticleContainer, article.Id, articleDTO.Content);
                await _articleRepository.CreateArticleAsync(article);

                var tagNames = new HashSet<string>(articleDTO.Tags ?? new List<string>());
                await _tagsService.CreateOrUpdateTagsAsync(tagNames, article.Id, article.Status);

                var authorStatistics = await _authorStatisticsRepository.GetAuthorStatisticsAsync(articleDTO.Author);

                if (authorStatistics == null)
                {
                    await _authorStatisticsRepository.CreateAuthorStatisticsAsync(articleDTO.Author);
                }

                _logger.LogInformation("Article with id {id} was created", article.Id);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<Article> UpdateArticleAsync(string articleId, ArticleUpdateDTO articleUpdateDTO, string userId)
        {
            try
            {
                var existingArticle = await _articleRepository.GetArticleByIdAsync(articleId);

                if (existingArticle == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                if (existingArticle.Author != userId)
                {
                    throw new CustomException(ErrorMessages.UpdateNotPermitted);
                }

                if (existingArticle.Status == ArticleStatus.Published)
                {
                    throw new CustomException(ErrorMessages.CantUpdatePublished);
                }

                var articleWithSameTitle = await _articleRepository.GetArticleByTitleAsync(articleUpdateDTO.Title);

                if (articleWithSameTitle != null && articleWithSameTitle.Id != articleId)
                {
                    throw new CustomException(ErrorMessages.ArticleWithThisTitleExists);
                }

                var updatedTagNames = new HashSet<string>(articleUpdateDTO.Tags ?? new List<string>());
                var existingTagNames = new HashSet<string>(existingArticle.Tags);
                var removedTagNames = existingTagNames.Except(updatedTagNames).ToHashSet();

                await _tagsService.RemoveArticleTagsAsync(removedTagNames, articleId, existingArticle.Status);
                await _tagsService.CreateOrUpdateTagsAsync(updatedTagNames, articleId, existingArticle.Status);

                await _blobStorageService.UploadHtmlContentAsync(_blobOptions.ArticleContainer, articleId, articleUpdateDTO.Content);

                _mapper.Map(articleUpdateDTO, existingArticle);
                await _articleRepository.UpdateArticleAsync(existingArticle);

                _logger.LogInformation("Article with id {articleId} was updated", articleId);

                return existingArticle;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task DeleteArticleAsync(string articleId)
        {
            try
            {
                var existingArticle = await _articleRepository.GetArticleByIdAsync(articleId);

                if (existingArticle == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                if (existingArticle.Status == ArticleStatus.Published)
                {
                    var authorStatistics = await _authorStatisticsRepository.GetAuthorStatisticsAsync(existingArticle.Author);
                    if (authorStatistics != null)
                    {
                        await _authorStatisticsRepository.UpdateAuthorStatisticsAsync(existingArticle.Author, -1);
                    }
                }

                var comments = await _commentsRepository.GetCommentsByArticleIdAsync(articleId);

                foreach (var comment in comments)
                {
                    await _commentsRepository.DeleteCommentAsync(comment);
                }

                await _articleRepository.DeleteArticleAsync(existingArticle);
                await _blobStorageService.DeleteHtmlContentAsync(_blobOptions.ArticleContainer, articleId);

                await _tagsService.RemoveArticleTagsAsync(existingArticle.Tags, articleId, existingArticle.Status);
                await _favoritesService.DeleteArticleFromFavorites(articleId);

                _logger.LogInformation("Article with id {articleId} was deleted", articleId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task ChangeArticleStatusToPublishedAsync(string articleId)
        {
            try
            {
                var article = await _articleRepository.GetArticleByIdAsync(articleId);

                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                var oldStatus = article.Status;
                await _articleRepository.ChangeArticleStatusToPublishedAsync(article);

                var tags = await _tagsRepository.GetTagsByArticleIdAsync(articleId);
                foreach (var tag in tags)
                {
                    tag.ArticleCount++;
                    await _tagsRepository.UpdateTagAsync(tag);
                }

                var authorStatistics = await _authorStatisticsRepository.GetAuthorStatisticsAsync(article.Author);
                if (authorStatistics != null)
                {
                    await _authorStatisticsRepository.UpdateAuthorStatisticsAsync(article.Author, 1);
                }
                else
                {
                    await _authorStatisticsRepository.CreateAuthorStatisticsAsync(article.Author);
                    await _authorStatisticsRepository.UpdateAuthorStatisticsAsync(article.Author, 1);
                }

                var user = await _userRepository.GetUserById(article.Author);
                await _emailService.ArticleIsPublished(user.Email, article.Title);

                _logger.LogInformation("Article with id {articleId} was published", articleId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> GetFavoriteArticles(int pageNumber, int pageSize, string orderBy, string userId)
        {
            try
            {
                var favorites = await _favoritesService.GetFavoritesIDs(userId);
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, articleIds: favorites);
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Articles = await MapArticles(articles.ToList()),
                    TotalCount = favorites.Count
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> FilterBySport(SportType sportType, int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = article => article.Sport == sportType && article.Status == ArticleStatus.Published;
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, predicate);
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Articles = await MapArticles(articles.ToList()),
                    TotalCount = await _articleRepository.GetCountofArticles(predicate)
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<OrderedArticlesDTO> FilterByAuthor(string authorId, int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = article => article.Author == authorId && article.Status == ArticleStatus.Published;
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy, predicate);
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Articles = await MapArticles(articles.ToList()),
                    TotalCount = await _articleRepository.GetCountofArticles(predicate)
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
