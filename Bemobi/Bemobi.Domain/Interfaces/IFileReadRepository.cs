namespace Bemobi.Domain.Interfaces
{
    public interface IFileReadRepository
    {
        Task<Domain.Entities.Files> GetByFileNameAsync(string filename);
        Task<List<Domain.Entities.Files>> GetByFileNameListAsync(List<string?> filenameList);
    }
}
