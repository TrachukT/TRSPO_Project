using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSport.Services.Interfaces
{
    public interface ILikesService
    {
        public Task AddLikeInfo(string articleId,string userId);
        public Task RemoveLikeInfo(string articleId,string userId);
        public Task<List<string>> GetLikeInfo(string userId);
        public Task<int> GetLikeCount(string articleId);
    }
}
