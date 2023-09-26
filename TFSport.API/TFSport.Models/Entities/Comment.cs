using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;

namespace TFSport.Models.Entities
{
    [PartitionKeyPath("/articleId")]
    public class Comment : FullItem
    {
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
