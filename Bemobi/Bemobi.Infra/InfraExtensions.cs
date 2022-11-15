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
        public const string server = "localhost";
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
        public static string GetConnectionString(this IConfiguration cfg)
        {
            string host = cfg["DBHOST"] ?? server, port = cfg["DBPORT"] ?? "3306",
            password = cfg["MYSQL_PASSWORD"] ?? cfg.GetConnectionString("MYSQL_PASSWORD"),
            userid = cfg["MYSQL_USER"] ?? cfg.GetConnectionString("MYSQL_USER"),
            db = cfg["MYSQL_DATABASE"] ?? cfg.GetConnectionString("MYSQL_DATABASE");

            return $"Server={host};Port={port};Database={db};Uid={userid};Pwd={password};";
        }
    }
}
