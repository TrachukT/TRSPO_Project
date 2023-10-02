using System.ComponentModel.DataAnnotations;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Articles
{
    public class ArticleUpdateDTO
    {
        [Required(ErrorMessage = ErrorMessages.TitleIsRequired)]
        [StringLength(70, MinimumLength = 15, ErrorMessage = ErrorMessages.TitleLength)]
        public string Title { get; set; }

        public SportType Sport { get; set; }

        [Required(ErrorMessage = ErrorMessages.DescriptionIsRequired)]
        [StringLength(150, MinimumLength = 50, ErrorMessage = ErrorMessages.DescriptionLength)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.NoImageProvided)]
        public string Image { get; set; }

        [MaxLength(5, ErrorMessage = ErrorMessages.TagsQuantity)]
        public List<string> Tags { get; set; }

        public string Content { get; set; }
    }
}
