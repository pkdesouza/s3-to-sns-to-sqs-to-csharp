using Bemobi.Domain.Events;
using Bemobi.Domain.Interfaces;

namespace Bemobi.Domain.Services
{
    public class S3EventDomainService : IS3EventDomainService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileReadRepository _fileReadRepository;
        public S3EventDomainService(
            IFileRepository fileRepository,
            IFileReadRepository fileReadRepository)
        {
            _fileReadRepository = fileReadRepository;
            _fileRepository = fileRepository;
        }

        public async Task SaveNotificationOnPut(S3PutObjectEvent @event)
        {
            var file = await _fileReadRepository.GetByFileNameAsync(@event.Name);

            if (file is null)
                await _fileRepository.UpdateAsync(new Entities.File(@event.Name, 0, DateTime.Now));
            else
                await _fileRepository.UpdateAsync(file);
        }
    }
}
