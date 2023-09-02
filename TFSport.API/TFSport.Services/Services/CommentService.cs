using Microsoft.Azure.CosmosRepository;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
	public class CommentService : ICommentService
	{
		private readonly IRepository<Comment> _commentRepo;

		public CommentService(IRepository<Comment> commentRepo)
		{
			_commentRepo = commentRepo;
		}
	}
}
