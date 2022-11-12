using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bemobi.Domain.Interfaces
{
    public interface IFileReadRepository
    {
        Task<Domain.Entities.Files> GetByFileNameAsync(string filename);
        Task<List<Domain.Entities.Files>> GetByFileNameListAsync(List<string> filenameList);
    }
}
