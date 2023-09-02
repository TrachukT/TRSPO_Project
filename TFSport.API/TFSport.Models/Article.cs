using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.Azure.CosmosRepository.Attributes;

namespace TFSport.Models
{
    [PartitionKeyPath("/partitionKey")]
    public class Article : BaseModel
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }
		
		[JsonProperty("status")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ArticleStatus Status { get; set; }

        [JsonIgnore]
        public static string ContainerName { get; set; }
    }
}
