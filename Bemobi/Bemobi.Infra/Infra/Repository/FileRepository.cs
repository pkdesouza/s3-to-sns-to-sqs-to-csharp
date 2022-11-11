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

        public async Task<Domain.Entities.File> AddAsync(Domain.Entities.File entity)
        {
            _context.Files.Add(entity);
            return await SaveChangesAsync(entity);
        }
        public async Task<Domain.Entities.File> UpdateAsync(Domain.Entities.File entity)
        {
            _context.Files.Update(entity);
            return await SaveChangesAsync(entity);
        }

        private async Task<Domain.Entities.File> SaveChangesAsync(Domain.Entities.File entity)
        {
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
