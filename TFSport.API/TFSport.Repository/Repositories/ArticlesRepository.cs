using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class ArticlesRepository : IArticlesRepository
    {
        private readonly IRepository<Article> _repository;

        public ArticlesRepository(IRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<List<Article>> GetArticlesInReview()
        {
            var articles = await _repository.GetAsync(x => x.Status == PostStatus.Review).ToListAsync();
            if (articles.Count == 0)
            {
                throw new CustomException(ErrorMessages.NoArticlesForReview);
            }
            return articles;
        }

        public async Task<List<Article>> GetAuthorsArticles(string authorId)
        {
            var articles = await _repository.GetAsync(x => x.Author == authorId).ToListAsync();
            if (articles.Count == 0)
            {
                throw new CustomException(ErrorMessages.NoAuthorsArticles);
            }
            return articles;
        }

        public async Task<List<Article>> GetPublishedArticles()
        {
            var articles = await _repository.GetAsync(x => x.Status == PostStatus.Published).ToListAsync();
            if (articles.Count == 0)
            {
                throw new CustomException(ErrorMessages.NoArticlesPublished);
            }
            return articles;
        }
    }
}
