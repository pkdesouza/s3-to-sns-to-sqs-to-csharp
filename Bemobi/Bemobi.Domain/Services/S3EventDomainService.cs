using Bemobi.Domain.Entities;
using Bemobi.Domain.Events.S3PutObject;
using Bemobi.Domain.Interfaces;
using System.Diagnostics;

namespace Bemobi.Domain.Services
{
    public class S3EventDomainService : IS3EventDomainService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly Func<Files, string, bool> IsSameFileName = (file, key) => file.FileName == key;
        private readonly Func<DateTime, DateTime, bool> DismissNotification = (lastModified, timestamp) => lastModified >= timestamp;
        public S3EventDomainService(
            IFileRepository fileRepository,
            IFileReadRepository fileReadRepository)
        {
            _fileReadRepository = fileReadRepository;
            _fileRepository = fileRepository;
        }

        public async Task SaveNotificationOnPutAsync(S3PutObjectEvent @event)
        {
            var fileNameList = @event.GetFileNameList();

            if (!fileNameList.Any()) return;

            var fileList = await _fileReadRepository.GetByFileNameListAsync(fileNameList);
            List<Files> updateList = new(), createList = new();

            FillUpdateAndCreateList(@event, fileList, updateList, createList);

            if (createList.Any())
                await _fileRepository.AddRangeAsync(createList);

            if (updateList.Any())
                await _fileRepository.UpdateRangeAsync(updateList);
        }

        private void FillUpdateAndCreateList(S3PutObjectEvent @event, List<Files> fileList, List<Files> updateList, List<Files> createList)
        {
            foreach (var (key, size, lastModified) in from record in @event!.RecordMessage!.Records
                                                      let key = record.GetFileName()
                                                      let size = record.GetFileSize()
                                                      let lastModified = @event!.Timestamp
                                                      select (key, size, lastModified))
            {
                if (fileList.Any(x => IsSameFileName(x, key)))
                {
                    var file = fileList.First(x => IsSameFileName(x, key));
                    if (DismissNotification(file.LastModified, @event.Timestamp))
                    {
                        Debug.WriteLine($"The lastModified field is newer than the notification, so the record will not be updated.");
                        continue;
                    }

                    updateList.Add(file.SetFileName(key).SetFileSize(size).SetLastModified(lastModified));
                    continue;
                }

                createList.Add(new Files(key, size, lastModified));
            }
        }
    }
}
