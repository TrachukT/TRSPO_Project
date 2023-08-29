﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TFSport.API.DTOModels.Users;
using TFSport.Models;

namespace TFSport.API.DTOModels.Articles
{
    public class GetArticlesListDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public UserDTO Author { get; set; }
        public PostStatus Status { get; set; }
		public DateTime UpdatedAt { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
