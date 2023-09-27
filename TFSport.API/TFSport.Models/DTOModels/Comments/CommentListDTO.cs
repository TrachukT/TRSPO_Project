namespace TFSport.Models.DTOModels.Comments
{
    public class CommentListDTO
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<CommentDTO> Comments { get; set; }
    }
}
