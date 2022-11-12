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

        public async Task<Files> AddAsync(Files entity)
        {
            await _context.Files.AddAsync(entity);
            return await SaveChangesAsync(entity);
        }

        public async Task AddRangeAsync(List<Files> entityList)
        {
            await _context.Files.AddRangeAsync(entityList);
            await SaveChangesAsync();
        }

        public async Task<Files> UpdateAsync(Files entity)
        {
            _context.Files.Update(entity);
            return await SaveChangesAsync(entity);
        }

        public async Task UpdateRangeAsync(List<Files> entityList)
        {
            _context.Files.UpdateRange(entityList);
            await SaveChangesAsync();
        }

        private async Task<Files> SaveChangesAsync(Files entity)
        {
            await _context.SaveChangesAsync();
            return entity;
        }

        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
