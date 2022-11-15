using Amazon.S3;
using Amazon.S3.Model;
using Bemobi.Domain.Interfaces;
using Bemobi.Infra.Infra.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Bemobi.Tests.Integrations
{
    public class S3PutObjectEventConsumerTests : IDisposable
    {
        private const string powershell = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe";
        private const string pathProject = @"C:\Projetos\s3-sns-sqs-csharp";
        private const int timeToProcess = 10000;
        private const string commandToDockerUp = "docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d";
        private const string commandToDockerStop = "docker-compose -f docker-compose.yml -f docker-compose.override.yml stop";
        private IConfiguration? _config;
        private readonly FileReadRepository _fileReadRepository;
        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }

        public S3PutObjectEventConsumerTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Configuration);
            _fileReadRepository = new FileReadRepository(Configuration);

            ProcessStartInfo processDockerUp = CreateProcessInfo(commandToDockerUp);
            StartProcessInfo(processDockerUp);

        }

        [Fact]
        public async Task Should_Success_Notification()
        {
            // Arrange
            string bucketName = "bemobi", keyName = "sample.txt", filePath = @"Mocks/Sample.txt";
            var resultBefore = await _fileReadRepository.GetByFileNameAsync(keyName);
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            PutObjectRequest putRequest = new()
            {
                BucketName = bucketName,
                Key = keyName,
                FilePath = filePath,
                ContentType = "text/plain"
            };

            // act 
            PutObjectResponse response = await client.PutObjectAsync(putRequest);
            await Task.Delay(20000);

            // Assert 
            var resultAfter = await _fileReadRepository.GetByFileNameAsync(keyName);
            if (resultBefore == null)
                Assert.NotNull(resultAfter);            
            else
                Assert.True(resultAfter.LastModified > resultBefore.LastModified);
        }

        void IDisposable.Dispose()
        {
            var processDockerStop = CreateProcessInfo(commandToDockerStop);
            StartProcessInfo(processDockerStop);
        }

        private static void StartProcessInfo(ProcessStartInfo processStart)
        {
            int exitCode;
            using var process = new Process();
            process.StartInfo = processStart;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit(timeToProcess);

            if (!process.HasExited)
            {
                process.Kill();
            }

            exitCode = process.ExitCode;
            process.Close();
        }
        private static ProcessStartInfo CreateProcessInfo(string cmd)
        {
            return new ProcessStartInfo(powershell, cmd)
            {
                CreateNoWindow = true,
                WorkingDirectory = pathProject,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }
    }
}
