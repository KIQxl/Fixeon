using Amazon.S3;
using Amazon.S3.Model;
using Fixeon.Shared.Configuration;

namespace Fixeon.Shared.Services
{
    public class MinIOStorageService : StorageService
    {
        public MinIOStorageService(StorageSettings settings)
        {
            _settings = settings;

            _client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, new AmazonS3Config
            {
                ServiceURL = settings.EndPoint,
                ForcePathStyle = settings.ForcePathStyle
            });
        }

        private readonly AmazonS3Client _client;
        private readonly StorageSettings _settings;

        public override async Task UploadFile(string filename, string contentType, Stream content)
        {
            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = filename,
                InputStream = content,
                ContentType = contentType,
                AutoCloseStream = true
            };

            await _client.PutObjectAsync(request);
        }


    }
}
