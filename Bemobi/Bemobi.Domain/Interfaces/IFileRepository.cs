namespace Bemobi.Domain.Interfaces
{
    public interface IFileRepository
    {
        Task<Entities.Files> AddAsync(Entities.Files entity);
        Task AddRangeAsync(List<Entities.Files> entityList);
        Task<Entities.Files> UpdateAsync(Entities.Files entity);
        Task UpdateRangeAsync(List<Entities.Files> entityList);
    }
}
