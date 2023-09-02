﻿using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;
namespace TFSport.Models
{
	[PartitionKeyPath("/articleId")]
	public class Comment : FullItem
	{
		[JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }

        [JsonIgnore]
        public static string ContainerName { get; set; }

        [JsonProperty("articleId")]
		public string ArticleId { get; set; }
		protected override string GetPartitionKeyValue()
		{
			return ArticleId;
		}
	}
}
