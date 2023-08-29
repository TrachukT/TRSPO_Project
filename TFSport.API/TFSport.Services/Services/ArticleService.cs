using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
	public class ArticleService : IArticleService
	{
		private readonly IRepository<Article> _articleRepo;

		public ArticleService(IRepository<Article> articleRepo)
		{
			_articleRepo = articleRepo;
		}
		
		public async Task CreateArticle()
		{
			
		}
	}
}
