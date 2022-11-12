using Bemobi.Domain.Interfaces;
using Bemobi.Infra.Infra.Context;
using Bemobi.Infra.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bemobi.Infra
{
    public static class InfraExtensions
    {
        public static IServiceCollection AddInfraDependency(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<BemobiContext>(options =>
               options.UseMySql(connectionString,
                   ServerVersion.AutoDetect(connectionString))
               );

            services.AddTransient<IFileReadRepository, FileReadRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            return services;
        }
    }
}
