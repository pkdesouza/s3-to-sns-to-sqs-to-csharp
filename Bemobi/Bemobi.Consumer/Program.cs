using Bemobi.Consumer;
using Bemobi.Domain;
using Bemobi.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDomainDependency();
        services.AddInfraDependency(hostContext.Configuration.GetConnectionString());
        services.AddMassTransitDependency(hostContext.Configuration);
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
