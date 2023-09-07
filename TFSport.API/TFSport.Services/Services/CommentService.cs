using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;
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
