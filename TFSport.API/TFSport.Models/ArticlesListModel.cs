using TFSport.API.DTOModels.Users;
using TFSport.Models;

namespace TFSport.API.DTOModels.Articles
{
    public class ArticlesListModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public UserInfo Author { get; set; }
        public ArticleStatus Status { get; set; }
		public DateTime UpdatedAt { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
