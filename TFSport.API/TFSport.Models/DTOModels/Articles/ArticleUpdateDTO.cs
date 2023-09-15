﻿using System.ComponentModel.DataAnnotations;
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
        public string Description { get; set; }

        [Required(ErrorMessage = ErrorMessages.NoImageProvided)]
        public string Image { get; set; }

        [MaxLength(5, ErrorMessage = ErrorMessages.TagsQuantity)]
        public List<string> Tags { get; set; }

        [StringLength(5000, MinimumLength = 200, ErrorMessage = ErrorMessages.ContentLength)]
        public string Content { get; set; }
    }
}
