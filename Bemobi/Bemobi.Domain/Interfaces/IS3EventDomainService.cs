using Bemobi.Domain.Events;

namespace Bemobi.Domain.Interfaces;
public interface IS3EventDomainService
{
    Task SaveNotificationOnPut(S3PutObjectEvent @event);
}