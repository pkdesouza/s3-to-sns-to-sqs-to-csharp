using Newtonsoft.Json;

namespace Bemobi.Domain.Events.S3PutObject
{
    public class OwnerIdentity
    {
        [JsonProperty("principalId")]
        public string? PrincipalId { get; set; }
    }
}
