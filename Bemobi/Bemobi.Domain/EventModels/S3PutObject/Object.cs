using Newtonsoft.Json;

namespace Bemobi.Domain.Events.S3PutObject
{
    public class Object
    {
        [JsonProperty("key")]
        public string? Key { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("eTag")]
        public string? ETag { get; set; }
        [JsonProperty("versionId")]
        public object? VersionId { get; set; }
        [JsonProperty("sequencer")]
        public string? Sequencer { get; set; }
    }
}
