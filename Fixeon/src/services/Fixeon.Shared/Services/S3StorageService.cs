using Amazon.S3;
using Fixeon.Shared.Configuration;

namespace Fixeon.Shared.Services
{
    public class S3StorageService : StorageService
    {
        private readonly AmazonS3Client _client = new AmazonS3Client();
        public S3StorageService(StorageSettings settings) { }

        public override Task<string> GetPresignedUrl(string filename)
        {
            throw new NotImplementedException();
        }

        public override async Task UploadFile(string filename, string contentType, Stream content)
        {
            throw new NotImplementedException();
        }
    }
}
