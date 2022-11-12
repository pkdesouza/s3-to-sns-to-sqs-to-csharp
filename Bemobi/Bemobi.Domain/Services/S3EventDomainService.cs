using Bemobi.Domain.Entities;
using Bemobi.Domain.Events;
using Bemobi.Domain.Interfaces;
using System.Diagnostics;

namespace Bemobi.Domain.Services
{
    public class S3EventDomainService : IS3EventDomainService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private Func<Files, string, bool> _getByKey = (file, key) => file.FileName == key;
        public S3EventDomainService(
            IFileRepository fileRepository,
            IFileReadRepository fileReadRepository)
        {
            _fileReadRepository = fileReadRepository;
            _fileRepository = fileRepository;
        }

        public async Task SaveNotificationOnPut(S3PutObjectEvent @event)
        {
            var fileNameList = @event.GetFileNameList();

            var fileList = await _fileReadRepository.GetByFileNameListAsync(fileNameList);
            List<Files> updateList = new(), createList = new();

            foreach (var record in @event.Message.Records)
            {
                var key = record.GetFileName();
                if (fileList.Any(x => _getByKey(x, key)))
                {
                    var file = fileList.First(x => _getByKey(x, key));
                    if (file.LastModified >= @event.Timestamp) {
                        Debug.WriteLine($"The lastModified field is newer than the notification, so the record will not be updated.");
                        continue;
                    }
                    updateList.Add(file);
                    continue;
                }
                
                createList.Add(new Files(record.GetFileName(), record.GetFileSize(), @event.Timestamp));
            }

            if (createList.Any())
                await _fileRepository.AddRangeAsync(createList);

            if (updateList.Any())
                await _fileRepository.UpdateRangeAsync(updateList);
        }
    }
}
