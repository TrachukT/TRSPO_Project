using TFSport.Models.DTOModels.Users;

namespace TFSport.Models.DTOModels.Comments
{
    public class CommentDTO
    {
        public UserInfo Author { get; set; }
        public string CommentId { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
