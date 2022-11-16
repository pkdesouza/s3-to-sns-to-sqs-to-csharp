using Newtonsoft.Json;

namespace Bemobi.Domain.Events.S3PutObject
{
    public class RequestParameters
    {
        [JsonProperty("sourceIPAddress")]
        public string? SourceIPAddress { get; set; }
    }
}
