using System.ComponentModel.DataAnnotations;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Articles
{
    public class ArticleUpdateDTO
    {
        [Required(ErrorMessage = ErrorMessages.TitleIsRequired)]
        public string Title { get; set; }

        public SportType Sport { get; set; }

        [Required(ErrorMessage = ErrorMessages.DescriptionIsRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.NoImageProvided)]
        public string Image { get; set; }

        public List<string> Tags { get; set; }

        public string Content { get; set; }
    }
}
