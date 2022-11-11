namespace Bemobi.Domain.Interfaces
{
    public interface IFileRepository
    {
        Task<Entities.File> AddAsync(Entities.File entity);
        Task<Entities.File> UpdateAsync(Entities.File entity);
    }
}
