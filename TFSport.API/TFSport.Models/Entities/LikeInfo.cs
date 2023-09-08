using Newtonsoft.Json;

namespace TFSport.Models.Entities
{
    public class LikeInfo
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("dateOfLike")]
        public DateTime DateOfLike { get; set; }
    }
}
