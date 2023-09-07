using AutoMapper;
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
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _configuration;

		public ArticleService(IArticlesRepository articleRepo, IUserService userService, IMapper mapper)
		{
			_articleRepository = articleRepo;
			_userService = userService;
			_mapper = mapper;
            _blobStorageService = blobStorageService;
			_configuration = configuration;
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
        public async Task<Article> CreateArticleAsync(Article article, string content)
        {
			try
			{
                var existingArticle = await _articleRepository.GetAsync(x => x.Title == article.Title).FirstOrDefaultAsync();

                if (existingArticle != null)
                {
                    throw new CustomException(ErrorMessages.ArticleWithThisTitleExists);
                }

                await _blobStorageService.UploadHtmlContentAsync(_configuration["BlobStorageContainers:ArticleContainer"], article.Id, content);

                article.UpdatedAt = DateTime.UtcNow;
                article.Status = ArticleStatus.Draft;

                await _articleRepository.CreateAsync(article, default);

                return article;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<string> GetArticleContentAsync(string articleId)
        {
            try
            {
                var article = await _articleRepository.GetAsync(articleId);

                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                var content = await _blobStorageService.GetHtmlContentAsync(_configuration["BlobStorageContainers:ArticleContainer"], articleId);

                return content;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

		public async Task<Article> GetArticleWithContentByIdAsync(string articleId)
		{
			try
			{
				var article = await _articleRepository.GetAsync(articleId);

                if (article == null)
				{
					throw new CustomException(ErrorMessages.ArticleDoesntExist);
				}

				return article;
			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}
		}

        public async Task<Article> UpdateArticleAsync(Article article, string content)
        {
            try
            {
                var existingArticle = await _articleRepository.GetAsync(x => x.Title == article.Title).FirstOrDefaultAsync();

                if (existingArticle != null && existingArticle.Id != article.Id)
                {
                    throw new CustomException(ErrorMessages.ArticleWithThisTitleExists);
                }

                await _blobStorageService.UploadHtmlContentAsync(_configuration["BlobStorageContainers:ArticleContainer"], article.Id, content);

                article.UpdatedAt = DateTime.UtcNow;
                article.Status = ArticleStatus.Draft;

                await _articleRepository.UpdateAsync(article);

                return article;
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
                var existingArticle = await _articleRepository.GetAsync(articleId);

                if (existingArticle == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                await _articleRepository.DeleteAsync(existingArticle);

                await _blobStorageService.DeleteHtmlContentAsync(_configuration["BlobStorageContainers:ArticleContainer"], articleId);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task ChangeArticleStatusToReviewAsync(string articleId)
        {
            try
            {
                var article = await _articleRepository.GetAsync(articleId);

                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                if (article.Status != ArticleStatus.Draft)
                {
                    throw new CustomException($"Article is currently in '{article.Status}' status and cannot be changed to 'Review'.");
                }

                article.Status = ArticleStatus.Review;
                await _articleRepository.UpdateAsync(article);
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
                var article = await _articleRepository.GetAsync(articleId);

                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                if (article.Status == ArticleStatus.Draft)
                {
                    throw new CustomException(ErrorMessages.ArticleNotSentForReview);
                }

                article.Status = ArticleStatus.Published;
                await _articleRepository.UpdateAsync(article);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
