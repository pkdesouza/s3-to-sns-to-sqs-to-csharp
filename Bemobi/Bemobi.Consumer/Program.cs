using Amazon.SQS;
using Bemobi.Consumer;
using Bemobi.Consumer.Consumers;
using Bemobi.Domain;
using Bemobi.Infra;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDomainDependency();
        services.AddInfraDependency(hostContext.Configuration["ConnectionStrings:Db"]);
        AddAwsConfig(services, hostContext.Configuration);
    })
    .ConfigureAppConfiguration((hostContext, configBuilder) =>
    {
        configBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    })
    .Build();

await host.RunAsync();


void AddAwsConfig(IServiceCollection services, IConfiguration configuration)
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
}