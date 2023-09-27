using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Extensions;
using System.Linq.Expressions;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;

namespace TFSport.Repository.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly IRepository<Comment> _repository;

        public CommentsRepository(IRepository<Comment> repository)
        {
            _repository = repository;
        }

        public async Task<Comment> GetCommentByIdAsync(string commentId)
        {
            var comment = await _repository.GetAsync(x => x.Id == commentId).FirstOrDefaultAsync();
            return comment;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            return await _repository.CreateAsync(comment);
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            await _repository.DeleteAsync(comment);
        }

        public async Task<HashSet<Comment>> GetCommentsByArticleIdAsync(string articleId)
        {
            var comments = await _repository.GetAsync(c => c.ArticleId == articleId);
            return comments.ToHashSet();
        }

        public async Task<IEnumerable<Comment>> GetCommentsPageAsync(Expression<Func<Comment, bool>> predicate, int pageNumber, int pageSize)
        {
            var comments = await _repository.PageAsync(predicate, pageNumber, pageSize);
            return comments.Items.ToList();
        }

        public async Task<int> GetCountofComments(Expression<Func<Comment, bool>> predicate)
        {
            var articles = await _repository.GetAsync(predicate).ToListAsync();
            return articles.Count;
        }
    }
}