using Amazon.S3;
using Amazon.S3.Model;
using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;

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
                ForcePathStyle = settings.ForcePathStyle,
                UseHttp = true
            });
        }

        private readonly AmazonS3Client _client;
        private readonly StorageSettings _settings;

        public override async Task UploadFile(string path, string filename, string contentType, Stream content)
        {
            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = $"{path}/{filename}",
                InputStream = content,
                ContentType = contentType,
                AutoCloseStream = true
            };

            await _client.PutObjectAsync(request);
        }

        public override async Task<string> GetPresignedUrl(string path, string filename)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = $"{path}/{filename}",
                Expires = DateTime.Now.AddHours(1)
            };

            return await _client.GetPreSignedURLAsync(request);
        }
    }
}
