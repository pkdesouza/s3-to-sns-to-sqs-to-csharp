using Newtonsoft.Json;

namespace Bemobi.Domain.Events
{
    public class S3PutObjectEvent
    {
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string TopicArn { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string SignatureVersion { get; set; }
        public string Signature { get; set; }
        public string SigningCertURL { get; set; }
        public string UnsubscribeURL { get; set; }
        public string Subject { get; set; }
        public RecordMessage RecordMessage { get; set; }

        public List<string> GetFileNameList() {
            return RecordMessage?.Records.Select(x => x.s3).Select(x => x.@object).Select(x => x.key).ToList() ?? new List<string>();
        }
    }
    public class Bucket
    {
        public string name { get; set; }
        public OwnerIdentity ownerIdentity { get; set; }
        public string arn { get; set; }
    }

    public class Object
    {
        public string key { get; set; }
        public int size { get; set; }
        public string eTag { get; set; }
        public object versionId { get; set; }
        public string sequencer { get; set; }
    }

    public class OwnerIdentity
    {
        public string principalId { get; set; }
    }

    public class Record
    {
        public string eventVersion { get; set; }
        public string eventSource { get; set; }
        public string awsRegion { get; set; }
        public DateTime eventTime { get; set; }
        public string eventName { get; set; }
        public UserIdentity userIdentity { get; set; }
        public RequestParameters requestParameters { get; set; }
        public ResponseElements responseElements { get; set; }
        public S3 s3 { get; set; }
        public string GetFileName()
        {
            return s3?.@object?.key?.ToString() ?? string.Empty;
        }
        public int GetFileSize()
        {
            return s3?.@object?.size ?? default;
        }
    }

    public class RequestParameters
    {
        public string sourceIPAddress { get; set; }
    }

    public class ResponseElements
    {
        [JsonProperty("x-amz-request-id")]
        public string XAmzRequestId { get; set; }

        [JsonProperty("x-amz-id-2")]
        public string XAmzId2 { get; set; }
    }

    public class RecordMessage
    {
        public List<Record> Records { get; set; }

    }

    public class S3
    {
        public string s3SchemaVersion { get; set; }
        public string configurationId { get; set; }
        public Bucket bucket { get; set; }
        
        [JsonProperty("object")]
        public Object @object { get; set; }
    }

    public class UserIdentity
    {
        public string principalId { get; set; }
    }
}
