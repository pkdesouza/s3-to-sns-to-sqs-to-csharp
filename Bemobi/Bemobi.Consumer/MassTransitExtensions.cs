using Amazon.SQS;
using Bemobi.Consumer.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bemobi.Consumer
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<S3PutObjectEventConsumer>();

                x.UsingAmazonSqs((context, cfg) =>
                {
                    cfg.Host(configuration["AWS:Region"], h =>
                    {
                        h.AccessKey(configuration["AWS:AccessKey"]);
                        h.SecretKey(configuration["AWS:SecretKey"]);

                    });

                    cfg.ReceiveEndpoint(configuration["AWS:Queues:S3EventConsumer"], c =>
                    {
                        c.ClearSerialization();
                        c.UseRawJsonSerializer();
                        c.PublishFaults = false;
                        c.ConfigureConsumeTopology = false;
                        c.WaitTimeSeconds = 20;
                        c.QueueAttributes.Add(QueueAttributeName.VisibilityTimeout, "30");

                        c.ConfigureConsumer<S3PutObjectEventConsumer>(context);
                    });
                });
            });

            services.AddOptions<MassTransitHostOptions>();
            services.AddHostedService<ConsumerHostedService>();

            return services;
        }
    }
}
