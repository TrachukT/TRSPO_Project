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
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticlesRepository _articleRepository;
        private readonly IUsersRepository _userRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly BlobStorageOptions _blobOptions;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly ITagsService _tagsService;

        public ArticleService(IOptions<BlobStorageOptions> blobOptions, IArticlesRepository articleRepository, IUsersRepository userRepository, 
            IUserService userService, IMapper mapper, IBlobStorageService blobStorageService, ILogger<ArticleService> logger, 
            IEmailService emailService, ITagsService tagsService, ITagsRepository tagsRepository)
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

        public async Task<OrderedArticlesDTO> PublishedArticles(int pageNumber, int pageSize, string orderBy)
        {
            try
            {
                Expression<Func<Article, bool>> predicate = article => article.Status == ArticleStatus.Published;
                var articles = await _articleRepository.GetArticles(pageNumber, pageSize, orderBy,predicate);
                var list = await MapArticles(articles.ToList());
                return new OrderedArticlesDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = await _articleRepository.GetCountofArticles(predicate),
                    Articles = list
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<IEnumerable<ArticleWithContentDTO>> GetArticlesByTagAsync(string tagName)
        {
            try
            {
                var tag = await _tagsRepository.GetTagAsync(tagName);
                if (tag != null)
                {
                    var articleIds = tag.ArticleIds;
                    var articles = await GetArticlesWithContentByIdsAsync(articleIds);
                    return articles;
                }
                return Enumerable.Empty<ArticleWithContentDTO>();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<IEnumerable<ArticleWithContentDTO>> SearchArticlesByTagsAsync(string substring)
        {
            try
            {
                var tagsContainingQuery = await _tagsRepository.GetTagsMatchingSubstringAsync(substring);
                var articleIds = tagsContainingQuery.SelectMany(tag => tag.ArticleIds).Distinct();

                var articles = await GetArticlesWithContentByIdsAsync(articleIds);
                return articles;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<List<ArticleWithContentDTO>> GetArticlesWithContentByIdsAsync(IEnumerable<string> articleIds)
        {
            var articles = new List<ArticleWithContentDTO>();
            foreach (var articleId in articleIds)
            {
                var article = await _articleRepository.GetArticleByIdAsync(articleId);
                if (article != null)
                {
                    var articleDto = await MapArticleWithContentAsync(article);
                    articles.Add(articleDto);
                }
            }
            return articles;
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

                await _blobStorageService.UploadHtmlContentAsync(_blobOptions.ArticleContainer, article.Id, articleDTO.Content);
                await _articleRepository.CreateArticleAsync(article);

                var tagNames = new HashSet<string>(articleDTO.Tags ?? new List<string>());
                await _tagsService.CreateOrUpdateTagsAsync(tagNames, article.Id);

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

                await _blobStorageService.UploadHtmlContentAsync(_blobOptions.ArticleContainer, articleId, articleUpdateDTO.Content);

                _mapper.Map(articleUpdateDTO, existingArticle);
                await _articleRepository.UpdateArticleAsync(existingArticle);

                var updatedTagNames = new HashSet<string>(articleUpdateDTO.Tags ?? new List<string>());
                var existingTagNames = new HashSet<string>(existingArticle.Tags);
                var removedTagNames = existingTagNames.Except(updatedTagNames).ToHashSet();

                await _tagsService.RemoveArticleTagsAsync(removedTagNames, articleId);
                await _tagsService.CreateOrUpdateTagsAsync(updatedTagNames, articleId);

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

                await _tagsService.RemoveArticleTagsAsync(existingArticle.Tags, articleId);

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

                await _articleRepository.ChangeArticleStatusToPublishedAsync(article);
                var user = await _userRepository.GetUserById(article.Author);
                await _emailService.ArticleIsPublished(user.Email, article.Title);

                _logger.LogInformation("Article with id {articleId} was published", articleId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
