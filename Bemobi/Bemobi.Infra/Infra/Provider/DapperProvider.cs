using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace Bemobi.Infra.Infra.Provider
{
    public class DapperProvider
    {
        private readonly IConfiguration _configuration;
        private static MySqlConnection? _connection;

        public DapperProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new MySqlConnection
            {
                ConnectionString = _configuration.GetConnectionString()
            };
        }

        protected IDbConnection Connection
        {
            get
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                    return _connection;
                
                _connection = new MySqlConnection
                {
                    ConnectionString = _configuration.GetConnectionString()
                };

                return _connection;
            }
        }
    }
}