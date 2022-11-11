using Bemobi.Domain.Interfaces;
using Bemobi.Infra.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Bemobi.Infra
{
    public static class InfraExtensions
    {
        public static IServiceCollection AddInfraDependency(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IFileReadRepository, FileReadRepository>();
            services.AddTransient<IFileRepository, FileRepository>();

            return services;
        }
    }
}
