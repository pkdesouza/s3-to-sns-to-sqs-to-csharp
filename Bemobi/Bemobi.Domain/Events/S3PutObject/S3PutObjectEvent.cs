namespace Bemobi.Domain.Events.S3PutObject
{
    public class S3PutObjectEvent
    {
        public string? Type { get; set; }
        public string? MessageId { get; set; }
        public string? TopicArn { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string? SignatureVersion { get; set; }
        public string? Signature { get; set; }
        public string? SigningCertURL { get; set; }
        public string? UnsubscribeURL { get; set; }
        public string? Subject { get; set; }
        public RecordMessage? RecordMessage { get; set; }

        public List<string?> GetFileNameList()
        {
            return RecordMessage?.Records?.Select(x => x.S3).Select(x => x?.Object).Select(x => x?.Key).ToList() ?? new List<string?>();
        }
    }
}
