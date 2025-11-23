using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Shared.Services
{
    public abstract class StorageService : IStorageServices
    {
        public abstract Task<string> GetPresignedUrl(string path, string filename);

        public abstract Task UploadFile(string folder, string filename, string contentType, Stream content);
    }
}
