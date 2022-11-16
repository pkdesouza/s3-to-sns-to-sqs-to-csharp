using Bemobi.Domain.Entities;
using Bemobi.Domain.Interfaces;
using Bemobi.Infra.Infra.Context;

namespace Bemobi.Infra.Infra.Repository
{
    public class FileRepository : IFileRepository
    {
        public readonly BemobiContext _context;

        public FileRepository(BemobiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddRangeAsync(List<Files> entityList)
        {
            await _context.Files.AddRangeAsync(entityList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<Files> entityList)
        {
            _context.Files.UpdateRange(entityList);
            await _context.SaveChangesAsync();
        }
    }
}
