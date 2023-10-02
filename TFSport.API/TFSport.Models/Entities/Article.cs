using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.Azure.CosmosRepository.Attributes;

namespace TFSport.Models.Entities
{
    [PartitionKeyPath("/partitionKey")]
    public class Article : BaseModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("sport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SportType Sport { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("tags")]
        public HashSet<string> Tags { get; set; }

        [JsonProperty("likeCount")]
        public int LikeCount { get; set; }

        [JsonProperty("likedUserIds")]
        public HashSet<string> LikedUserIds { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ArticleStatus Status { get; set; }
    }
}
