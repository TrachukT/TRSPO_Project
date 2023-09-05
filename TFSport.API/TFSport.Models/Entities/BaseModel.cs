using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Attributes;
using Newtonsoft.Json;

namespace TFSport.Models.Entities
{
    [PartitionKeyPath("/partitionKey")]
    public class BaseModel : FullItem
    {
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }
        protected override string GetPartitionKeyValue()
        {
            return PartitionKey;
        }
    }
}
