using Newtonsoft.Json;

namespace Bemobi.Domain.Events.S3PutObject
{
    public class ResponseElements
    {
        [JsonProperty("x-amz-request-id")]
        public string? XAmzRequestId { get; set; }

        [JsonProperty("x-amz-id-2")]
        public string? XAmzId2 { get; set; }
    }
}
