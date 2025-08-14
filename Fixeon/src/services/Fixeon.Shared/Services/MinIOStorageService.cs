using Amazon.S3;
using Amazon.S3.Model;
using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Shared.Services
{
    public class MinIOStorageService : StorageService
    {
        public MinIOStorageService(StorageSettings settings, ITenantContext tenantContext)
        {
            _settings = settings;

            _client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, new AmazonS3Config
            {
                ServiceURL = settings.EndPoint,
                ForcePathStyle = settings.ForcePathStyle,
                UseHttp = true
            });
            _tenantContext = tenantContext;
        }

        private readonly AmazonS3Client _client;
        private readonly StorageSettings _settings;
        private readonly ITenantContext _tenantContext;

        public override async Task UploadFile(string filename, string contentType, Stream content)
        {
            var request = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = $"{_tenantContext.TenantId}/{filename}",
                InputStream = content,
                ContentType = contentType,
                AutoCloseStream = true
            };

            await _client.PutObjectAsync(request);
        }

        public override async Task<string> GetPresignedUrl(string filename)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = filename,
                Expires = DateTime.Now.AddMinutes(30)
            };

            return await _client.GetPreSignedURLAsync(request);
        }
    }
}
