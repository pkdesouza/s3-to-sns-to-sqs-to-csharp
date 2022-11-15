using Bemobi.Domain.Entities;
using Bemobi.Domain.Events.S3PutObject;
using Bemobi.Domain.Interfaces;
using Bemobi.Domain.Services;
using Moq;
using Newtonsoft.Json;
using Record = Bemobi.Domain.Events.S3PutObject.Record;

namespace Bemobi.Tests.Units
{
    public class S3EventDomainServiceTests
    {
        private readonly Mock<IFileRepository> _fileRepository;
        private readonly Mock<IFileReadRepository> _fileReadRepository;
        private const string _pathMock = @"Mocks/Valid_SaveNotificationOnPut.json";
        private List<Files> _filesUpdated = new(), _filesCreated = new();
        private readonly S3PutObjectEvent _validEventInput;
        private readonly Record _message;


        public S3EventDomainServiceTests()
        {
            _fileRepository = new Mock<IFileRepository>();
            _fileReadRepository = new Mock<IFileReadRepository>();
            _validEventInput = GetMockByFile(_pathMock);
            if (!(_validEventInput != null && _validEventInput.RecordMessage != null && _validEventInput.RecordMessage.Records != null))
            {
                Assert.NotNull(null);
            }
            _message = _validEventInput!.RecordMessage!.Records!.First();
        }

        [Fact]
        public async Task Should_Add_SaveNotificationOnPut()
        {
            // Arrange
            _filesUpdated = new(); _filesCreated = new();

            // Setups
            _fileRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<Files>>())).Callback<List<Files>>(r => _filesCreated = r);
            _fileRepository.Setup(x => x.UpdateRangeAsync(_filesUpdated));
            _fileReadRepository.Setup(x => x.GetByFileNameListAsync(_validEventInput.GetFileNameList())).ReturnsAsync(_filesUpdated);

            // Act
            var s3EventDomainService = new S3EventDomainService(_fileRepository.Object, _fileReadRepository.Object);
            await s3EventDomainService.SaveNotificationOnPutAsync(_validEventInput);

            // Assert
            _fileRepository.Verify(x => x.UpdateRangeAsync(_filesUpdated), Times.Never());
            _fileRepository.Verify(x => x.AddRangeAsync(_filesCreated), Times.AtLeastOnce());
            Assert.Empty(_filesUpdated);
            Assert.NotEmpty(_filesCreated);
            Assert.Equal(_filesCreated.First().FileName, _message.GetFileName());
            Assert.Equal(_filesCreated.First().FileSize, _message.GetFileSize());
            Assert.Equal(_filesCreated.First().LastModified, _validEventInput.Timestamp);
        }
        [Fact]
        public async Task Should_Update_SaveNotificationOnPut()
        {
            // Arrange
            _filesUpdated = new(); _filesCreated = new();
            int size = new Random().Next(1, 5000);

            // Setups
            _filesUpdated.Add(new Files(_message.GetFileName(), size, _validEventInput.Timestamp.AddMinutes(-60)));
            _fileRepository.Setup(x => x.AddRangeAsync(_filesCreated));
            _fileRepository.Setup(x => x.UpdateRangeAsync(It.IsAny<List<Files>>())).Callback<List<Files>>(r => _filesUpdated = r);
            _fileReadRepository.Setup(x => x.GetByFileNameListAsync(_validEventInput.GetFileNameList())).ReturnsAsync(_filesUpdated);

            // Act
            var s3EventDomainService = new S3EventDomainService(_fileRepository.Object, _fileReadRepository.Object);
            await s3EventDomainService.SaveNotificationOnPutAsync(_validEventInput);

            // Assert
            _fileRepository.Verify(x => x.UpdateRangeAsync(_filesUpdated), Times.AtLeastOnce());
            _fileRepository.Verify(x => x.AddRangeAsync(_filesCreated), Times.Never());
            Assert.Empty(_filesCreated);
            Assert.NotEmpty(_filesUpdated);
            Assert.Equal(_filesUpdated.First().FileName, _message.GetFileName());
            Assert.Equal(_filesUpdated.First().FileSize, _message.GetFileSize());
            Assert.Equal(_filesUpdated.First().LastModified, _validEventInput.Timestamp);

        }
        [Fact]
        public async Task Should_Dimiss_SaveNotificationOnPut()
        {
            // Arrange
            _filesUpdated = new(); _filesCreated = new();

            // Setups
            _filesUpdated.Add(new Files(_message.GetFileName(), _message.GetFileSize(), _validEventInput.Timestamp.AddMinutes(60)));
            _fileRepository.Setup(x => x.AddRangeAsync(_filesCreated));
            _fileRepository.Setup(x => x.UpdateRangeAsync(It.IsAny<List<Files>>()));
            _fileReadRepository.Setup(x => x.GetByFileNameListAsync(_validEventInput.GetFileNameList())).ReturnsAsync(_filesUpdated);

            // Act
            var s3EventDomainService = new S3EventDomainService(_fileRepository.Object, _fileReadRepository.Object);
            await s3EventDomainService.SaveNotificationOnPutAsync(_validEventInput);

            // Assert
            _fileRepository.Verify(x => x.UpdateRangeAsync(_filesUpdated), Times.Never());
            _fileRepository.Verify(x => x.AddRangeAsync(_filesCreated), Times.Never());

        }
        private static S3PutObjectEvent GetMockByFile(string path)
        {
            var validEventInput = JsonConvert.DeserializeObject<S3PutObjectEvent>(File.ReadAllText(path));

            if (validEventInput == null)
            {
                Assert.NotNull(validEventInput);
                throw new Exception();
            }

            validEventInput.RecordMessage = JsonConvert.DeserializeObject<RecordMessage>(validEventInput.Message!);

            if (validEventInput.RecordMessage == null)
            {
                Assert.NotNull(validEventInput.RecordMessage);
                return validEventInput;
            }
            return validEventInput;
        }
    }
}
