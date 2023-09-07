using System.ComponentModel.DataAnnotations;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Articles
{
    public class ArticleUpdateDTO
    {
        [Required(ErrorMessage = ErrorMessages.TitleIsRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.DescriptionIsRequired)]
        public string Description { get; set; }

        public string Content { get; set; }
    }
}
