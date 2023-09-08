using System.ComponentModel.DataAnnotations;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Articles
{
    public class ArticleCreateDTO
    {
        [Required(ErrorMessage = ErrorMessages.TitleIsRequired)]
        public string Title { get; set; }

        public string Sport { get; set; }

        [Required(ErrorMessage = ErrorMessages.DescriptionIsRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.AuthorIsRequired)]
        [RegularExpression(@"^(\{{0,1}\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\}{0,1})$", ErrorMessage = ErrorMessages.InvalidAuthorId)]
        public string Author { get; set; }

        [Required(ErrorMessage = ErrorMessages.NoImageProvided)]
        public string Image {  get; set; }

        public List<string> Tags { get; set; }

        public string Content { get; set; }
    }
}
