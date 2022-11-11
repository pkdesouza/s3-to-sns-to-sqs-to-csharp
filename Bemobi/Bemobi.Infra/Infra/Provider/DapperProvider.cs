using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace Bemobi.Infra.Infra.Provider
{
    public class DapperProvider
    {
        private readonly IConfiguration _configuration;

        public DapperProvider(IConfiguration configuration) => _configuration = configuration;

        protected IDbConnection Connection
        {
            get
            {
                return null; //UseMySql(_configuration.GetSection("ConnectionString").Value);
            }
        }
    }
}