using Bemobi.Domain.Interfaces;
using Bemobi.Infra.Infra.Provider;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Bemobi.Infra.Infra.Repository
{
    public class FileReadRepository : DapperProvider, IFileReadRepository
    {
        public FileReadRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Domain.Entities.File> GetByFileNameAsync(string filename)
        {
            return await Connection.QueryFirstOrDefaultAsync<Domain.Entities.File>(
                 "SELECT f.filename, f.filesize, f.lastmodified FROM files f where f.filename = @filename",
                 new { filename }
                 );
        }
    }
}
