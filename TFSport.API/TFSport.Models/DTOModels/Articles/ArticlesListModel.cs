using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TFSport.Models.DTOModels.Users;
using TFSport.Models.Entities;

namespace TFSport.Models.DTOModels.Articles
{
    public class ArticlesListModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserInfo Author { get; set; }
        public PostStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
