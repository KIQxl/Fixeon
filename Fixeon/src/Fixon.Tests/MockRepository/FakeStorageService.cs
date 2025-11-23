using Fixeon.Shared.Core.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeStorageService : IStorageServices
    {
        public async Task<string> GetPresignedUrl(string path, string filename)
        {
            return string.Empty;
        }

        public Task UploadFile(string folder, string filename, string contentType, Stream content)
        {
            throw new NotImplementedException();
        }
    }
}
