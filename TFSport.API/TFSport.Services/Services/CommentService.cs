using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;
using TFSport.Models.DTOModels.Comments;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using System.Drawing.Printing;
using System.Linq.Expressions;
using TFSport.Models.DTOModels.Users;
using TFSport.Repository.Repositories;

namespace TFSport.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentsRepository _commentRepository;
        private readonly IArticlesRepository _articleRepository;
        private readonly IUsersRepository _usersRepository;

        public CommentService(ICommentsRepository commentRepository, IArticlesRepository articleRepository, IUsersRepository usersRepository)
        {
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByArticleIdAsync(string articleId, int pageNumber, int pageSize)
        {
            try
            {
                Expression<Func<Comment, bool>> predicate = c => c.ArticleId == articleId;
                var comments = await _commentRepository.GetCommentsPageAsync(predicate, pageNumber, pageSize);

                var commentDTOs = new List<CommentDTO>();

                foreach (var comment in comments)
                {
                    var user = await _usersRepository.GetUserById(comment.Author);

                    var commentDTO = new CommentDTO
                    {
                        Author = new UserInfo
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName
                        },
                        CommentId = comment.Id,
                        Content = comment.Content,
                        CreatedAt = comment.CreatedTimeUtc
                    };

                    commentDTOs.Add(commentDTO);
                }

                return commentDTOs;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<Comment> AddCommentAsync(CommentCreateDTO commentDto, string articleId, string userId)
        {
            try
            {
                var article = await _articleRepository.GetArticleByIdAsync(articleId);
                if (article == null)
                {
                    throw new CustomException(ErrorMessages.ArticleDoesntExist);
                }

                var comment = new Comment
                {
                    Author = userId,
                    Content = commentDto.Content,
                    ArticleId = articleId
                };

                var createdComment = await _commentRepository.CreateCommentAsync(comment);

                return createdComment;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task DeleteCommentAsync(string commentId, string userId)
        {
            try
            {
                var existingComment = await _commentRepository.GetCommentByIdAsync(commentId);
                if (existingComment == null)
                {
                    throw new CustomException(ErrorMessages.CommentDoesntExist);
                }

                var user = await _usersRepository.GetUserById(userId);
                if (existingComment.Author != userId)
                {
                    if (user.UserRole != UserRoles.SuperAdmin)
                    {
                        throw new CustomException(ErrorMessages.UnauthorizedToDeleteComment);
                    }
                }

                await _commentRepository.DeleteCommentAsync(existingComment);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}