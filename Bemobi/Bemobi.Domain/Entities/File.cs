using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bemobi.Domain.Entities
{
    [Table("files")]
    public class Files
    {
        public Files(string fileName, long fileSize, DateTime lastModified)
        {
            FileName = fileName;
            FileSize = fileSize;
            LastModified = lastModified;
        }

        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime LastModified { get; set; }
    }
}
