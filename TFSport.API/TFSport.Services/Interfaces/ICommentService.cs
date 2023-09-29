using TFSport.Models.DTOModels.Comments;
using TFSport.Models.Entities;

namespace TFSport.Services.Interfaces
{
	public interface ICommentService
	{
        public Task<CommentListDTO> GetCommentsByArticleIdAsync(string articleId, int pageNumber, int pageSize);

        public Task<CommentDTO> AddCommentAsync(CommentCreateDTO commentDto, string articleId, string userId);

        public Task DeleteCommentAsync(string commentId, string userId);
    }
}
