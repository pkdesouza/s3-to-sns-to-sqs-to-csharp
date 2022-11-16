using Newtonsoft.Json;

namespace Bemobi.Domain.Events.S3PutObject
{
    public class Bucket
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("ownerIdentity")]
        public OwnerIdentity? OwnerIdentity { get; set; }
        [JsonProperty("arn")]
        public string? Arn { get; set; }
    }
}
