using Amazon.S3;
using Amazon.S3.Model;
using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Shared.Services
{
    public class S3StorageService : StorageService
    {
        private readonly AmazonS3Client _client;
        private readonly StorageSettings _settings;
        private readonly ITenantContextServices _tenantContext;
        public S3StorageService(StorageSettings settings, ITenantContextServices tenantContext)
        {
            _settings = settings;
            var config = new AmazonS3Config
            {
                ServiceURL = settings.EndPoint,
                ForcePathStyle = settings.ForcePathStyle,
                AuthenticationRegion = settings.Region
            };

            _client = new AmazonS3Client(settings.AccessKey, settings.SecretKey, config);
        }

        public override async Task<string> GetPresignedUrl(string filename)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = filename,
                Expires = System.DateTime.Now.AddHours(1)
            };

            string url = await _client.GetPreSignedURLAsync(request);
            return url;
        }

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
    }
}
