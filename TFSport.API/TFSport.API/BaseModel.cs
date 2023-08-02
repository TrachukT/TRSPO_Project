using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Attributes;

namespace TFSport.API
{
	[PartitionKeyPath("/partitionKey")]
	public class BaseModel:FullItem
	{
		public string Id { get; set; }
		public string PartitionKey { get; set; }
		protected override string GetPartitionKeyValue()
		{
			return PartitionKey;
		}
	}
}
