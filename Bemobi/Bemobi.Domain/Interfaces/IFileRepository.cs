namespace Bemobi.Domain.Interfaces
{
    public interface IFileRepository
    {
        Task AddRangeAsync(List<Entities.Files> entityList);
        Task UpdateRangeAsync(List<Entities.Files> entityList);
    }
}
