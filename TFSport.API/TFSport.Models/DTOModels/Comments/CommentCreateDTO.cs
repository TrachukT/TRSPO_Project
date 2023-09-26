using System.ComponentModel.DataAnnotations;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Comments
{
    public class CommentCreateDTO
    {
        [Required(ErrorMessage = ErrorMessages.ContentIsRequired)]
        public string Content { get; set; }
    }
}
