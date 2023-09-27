using System.Linq.Expressions;
using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface ICommentsRepository
    {
        public Task<Comment> GetCommentByIdAsync(string commentId);

        public Task<Comment> CreateCommentAsync(Comment comment);

        public Task DeleteCommentAsync(Comment comment);

        public Task<HashSet<Comment>> GetCommentsByArticleIdAsync(string articleId);

        public Task<IEnumerable<Comment>> GetCommentsPageAsync(Expression<Func<Comment, bool>> predicate, int pageNumber, int pageSize);

        public Task<int> GetCountofComments(Expression<Func<Comment, bool>> predicate);
    }
}
