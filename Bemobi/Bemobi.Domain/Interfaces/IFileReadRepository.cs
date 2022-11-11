using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bemobi.Domain.Interfaces
{
    public interface IFileReadRepository
    {
        Task<Domain.Entities.File> GetByFileNameAsync(string filename);
    }
}
