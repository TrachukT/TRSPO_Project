using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;

namespace TFSport.Models.Entities
{
    [PartitionKeyPath("/partitionKey")]
    public class Tag : BaseModel
    {
        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("articleId")]
        public List<string> ArticleId { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
