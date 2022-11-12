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

        public string FileName { get; private set; }
        public long FileSize { get; private set; }
        public DateTime LastModified { get; private set; }

        public Files SetFileName(string value)
        {
            FileName = value;
            return this;
        }
        public Files SetFileSize(int value)
        {
            FileSize = value;
            return this;
        }
        public Files SetLastModified(DateTime value)
        {
            LastModified = value;
            return this;
        }
    }
}
