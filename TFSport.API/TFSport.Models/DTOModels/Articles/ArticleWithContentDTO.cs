using TFSport.Models.Entities;

namespace TFSport.Models.DTOModels.Articles
{
    public class ArticleWithContentDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Author { get; set; }

        public ArticleStatus Status { get; set; }

        public string Content { get; set; }
    }
}
