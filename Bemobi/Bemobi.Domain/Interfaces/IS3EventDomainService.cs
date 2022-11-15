using Bemobi.Domain.Events.S3PutObject;

namespace Bemobi.Domain.Interfaces;
public interface IS3EventDomainService
{
    Task SaveNotificationOnPutAsync(S3PutObjectEvent @event);
}