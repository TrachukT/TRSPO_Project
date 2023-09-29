using TFSport.Repository.Interfaces;
using TFSport.Services.Interfaces;
using TFSport.Models.DTOModels.Comments;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;
using System.Linq.Expressions;
using TFSport.Models.DTOModels.Users;

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

        public async Task<CommentListDTO> GetCommentsByArticleIdAsync(string articleId, int pageNumber, int pageSize)
        {
            try
            {
                Expression<Func<Comment, bool>> predicate = c => c.ArticleId == articleId;

                int totalCount = await _commentRepository.GetCountofComments(predicate);

                var comments = await _commentRepository.GetCommentsPageAsync(predicate, 1, int.MaxValue);
                comments = comments.OrderByDescending(comment => comment.CreatedTimeUtc).ToList();

                var startIndex = (pageNumber - 1) * pageSize;
                comments = comments.Skip(startIndex).Take(pageSize).ToList();

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

                return new CommentListDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Comments = commentDTOs
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<CommentDTO> AddCommentAsync(CommentCreateDTO commentDto, string articleId, string userId)
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

                var user = await _usersRepository.GetUserById(userId);

                var commentDTO = new CommentDTO
                {
                    Author = new UserInfo
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    },
                    CommentId = createdComment.Id,
                    Content = createdComment.Content,
                    CreatedAt = createdComment.CreatedTimeUtc
                };

                return commentDTO;
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