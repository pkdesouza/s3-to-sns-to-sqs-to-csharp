using Bemobi.Domain.Entities;
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

        public async Task<Files> GetByFileNameAsync(string filename)
        {
            return await Connection.QueryFirstOrDefaultAsync<Files>(
                 "SELECT f.filename, f.filesize, f.lastmodified FROM files f where f.filename = @filename",
                 new { filename }
                 );
        }

        public async Task<List<Files>> GetByFileNameListAsync(List<string?> fileNameList)
        {
            return (await Connection.QueryAsync<Files>(
                 "SELECT f.filename, f.filesize, f.lastmodified FROM files f where f.filename in (@fileNameList)",
                 new { fileNameList }
                 )).ToList();
        }
    }
}
