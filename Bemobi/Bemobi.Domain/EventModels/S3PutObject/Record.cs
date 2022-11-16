using Newtonsoft.Json;

namespace Bemobi.Domain.Events.S3PutObject
{
    public class Record
    {
        [JsonProperty("eventVersion")]
        public string? EventVersion { get; set; }
        [JsonProperty("eventSource")]
        public string? EventSource { get; set; }
        [JsonProperty("awsRegion")]
        public string? AwsRegion { get; set; }
        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; }
        [JsonProperty("eventName")]
        public string? EventName { get; set; }
        [JsonProperty("userIdentity")]
        public UserIdentity? UserIdentity { get; set; }
        [JsonProperty("requestParameters")]
        public RequestParameters? RequestParameters { get; set; }
        [JsonProperty("responseElements")]
        public ResponseElements? ResponseElements { get; set; }
        [JsonProperty("s3")]
        public S3? S3 { get; set; }
        public string GetFileName() => S3?.Object?.Key?.ToString() ?? string.Empty;
        public int GetFileSize() => S3?.Object?.Size ?? default;
    }
}
