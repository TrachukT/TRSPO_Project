using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSport.Models
{
	[PartitionKeyPath("/articleId")]
	public class Comment:FullItem
	{
		[JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }
		
		[JsonProperty("articleId")]
		public string ArticleId { get; set; }
		protected override string GetPartitionKeyValue()
		{
			return ArticleId;
		}

	}
}
