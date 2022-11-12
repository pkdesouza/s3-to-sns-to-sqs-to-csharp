using Bemobi.Consumer.Consumers.Base;
using Bemobi.Domain.Events;
using Bemobi.Domain.Interfaces;
using MassTransit;

namespace Bemobi.Consumer.Consumers
{
    internal class S3PutObjectEventConsumer : BaseEventConsumer, IConsumer<S3PutObjectEvent>
    {
        public static async Task ProcessEventAsync(IS3EventDomainService domainService, ConsumeContext<S3PutObjectEvent> context)
        {
            if (context.Message != null)
                await domainService.SaveNotificationOnPut(context.Message);
        }

        public async Task Consume(ConsumeContext<S3PutObjectEvent> context)
        {
            await ProcessAsync<S3PutObjectEvent, IS3EventDomainService>(
                context,
                GetType().Name,
                (eventProcess) => ProcessEventAsync(eventProcess, context));
        }
    }
}
