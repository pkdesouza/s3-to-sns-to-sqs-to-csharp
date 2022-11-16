using Newtonsoft.Json;

namespace Bemobi.Domain.Events.S3PutObject
{
    public class S3
    {
        [JsonProperty("s3SchemaVersion")]
        public string? S3SchemaVersion { get; set; }
        [JsonProperty("configurationId")]
        public string? ConfigurationId { get; set; }
        [JsonProperty("bucket")]
        public Bucket? Bucket { get; set; }
        [JsonProperty("object")]
        public Object? Object { get; set; }
    }
}
