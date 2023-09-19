namespace TFSport.Models.DTOModels.Articles
{
    public class OrderedArticlesDTO
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<ArticlesListModel> Articles { get; set; }
    }
}
