using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models.Entities;

namespace TFSport.Repository.Interfaces
{
    public interface IArticlesRepository
    {
        public Task<List<Article>> GetPublishedArticles();
        public Task<List<Article>> GetAuthorsArticles(string authorId);
        public Task<List<Article>> GetArticlesInReview();
    }
}
