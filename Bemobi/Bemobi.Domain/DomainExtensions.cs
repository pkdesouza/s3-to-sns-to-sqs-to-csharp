using Bemobi.Domain.Interfaces;
using Bemobi.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bemobi.Domain
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainDependency(this IServiceCollection services)
        {
            services.AddTransient<IS3EventDomainService, S3EventDomainService>();

            return services;
        }
    }
}
