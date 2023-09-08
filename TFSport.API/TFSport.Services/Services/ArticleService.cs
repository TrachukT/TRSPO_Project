using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TFSport.Models;
using TFSport.Models.DTOModels.Articles;
using TFSport.Models.DTOModels.Users;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticlesRepository _articleRepository;
        private readonly IUsersRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly BlobStorageOptions _blobOptions;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public ArticleService(IOptions<BlobStorageOptions> blobOptions, IArticlesRepository articleRepository, IUsersRepository userRepository, IUserService userService, IMapper mapper, IBlobStorageService blobStorageService, ILogger<ArticleService> logger, IMemoryCache memoryCache)
        {
            _articleRepository = articleRepository;
            _userService = userService;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _blobOptions = blobOptions.Value;
            _logger = logger;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<List<ArticlesListModel>> ArticlesForApprove()
        {
            try
            {
                var articles = await _articleRepository.GetArticlesInReview();
                var list = await MapArticles(articles);
                return list;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<ArticlesListModel>> AuthorsArticles(string authorId)
        {
            try
            {
                var articles = await _articleRepository.GetAuthorsArticles(authorId);
                var list = await MapArticles(articles);
                return list;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<ArticlesListModel>> PublishedArticles()
        {
            try
            {
                var articles = await _articleRepository.GetPublishedArticles();
                var list = await MapArticles(articles);
                return list;
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

                var content = await _blobStorageService.GetHtmlContentAsync(_blobOptions.ArticleContainer, article.Id);
                var user = await _userService.GetUserById(article.Author);
                var userDTO = _mapper.Map<UserInfo>(user);

                var articleWithContentDTO = _mapper.Map<ArticleWithContentDTO>(article);
                articleWithContentDTO.Content = content;
                articleWithContentDTO.Author = userDTO;

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

                await _blobStorageService.UploadHtmlContentAsync(_blobOptions.ArticleContainer, article.Id, articleDTO.Content);
                await _articleRepository.CreateArticleAsync(article);

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

                var articleWithSameTitle = await _articleRepository.GetArticleByTitleAsync(articleUpdateDTO.Title);

                if (articleWithSameTitle != null && articleWithSameTitle.Id != articleId)
                {
                    throw new CustomException(ErrorMessages.ArticleWithThisTitleExists);
                }

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

                await _articleRepository.DeleteArticleAsync(existingArticle);

                await _blobStorageService.DeleteHtmlContentAsync(_blobOptions.ArticleContainer, articleId);
                _logger.LogInformation("Article with id {articleId} was deleted", articleId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task ChangeArticleStatusToReviewAsync(string articleId, string userId)
        {
            try
            {
                var article = await _articleRepository.GetArticleByIdAsync(articleId);

                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                if (article.Status != ArticleStatus.Draft)
                {
                    throw new CustomException($"Article is currently in '{article.Status}' status and cannot be changed to 'Review'.");
                }

                if (article.Author != userId)
                {
                    throw new CustomException(ErrorMessages.ChangeStatusNotPermitted);
                }

                await _articleRepository.ChangeArticleStatusToReviewAsync(article);

                _logger.LogInformation("Article with id {articleId} was sent to review", articleId);
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

                if (article.Status == ArticleStatus.Draft)
                {
                    throw new CustomException(ErrorMessages.ArticleNotSentForReview);
                }

                await _articleRepository.ChangeArticleStatusToPublishedAsync(article);

                _logger.LogInformation("Article with id {articleId} was published", articleId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<SportType>> GetSportTypes()
        {
            var isCached = _memoryCache.TryGetValue(nameof(SportType), out List<SportType> sportsList);
            if (!isCached)
            {
                sportsList = new List<SportType>();
                var sportsValues = Enum.GetValues(typeof(SportType));
                foreach (var value in sportsValues)
                {
                    sportsList.Add((SportType)value);
                }
                _memoryCache.Set(nameof(SportType), sportsList, new MemoryCacheEntryOptions()
                    .SetSize(5)
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            return sportsList;
        }

    }
}
