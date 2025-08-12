using Fixeon.Shared.Core.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeStorageService : IStorageServices
    {
        public Task<string> GetPresignedUrl(string filename)
        {
            throw new NotImplementedException();
        }

        public Task UploadFile(string filename, string contentType, Stream content)
        {
            throw new NotImplementedException();
        }
    }
}
