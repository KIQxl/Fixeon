using Fixeon.Shared.Core.Interfaces;

namespace Fixeon.Shared.Services
{
    public abstract class StorageService : IStorageServices
    {
        public abstract Task UploadFile(string filename, string contentType, Stream content);
    }
}
