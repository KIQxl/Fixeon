using Fixeon.Shared.Core.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeStorageService : IStorageServices
    {
        public async Task<string> GetPresignedUrl(string filename)
        {
            return string.Empty;
        }

        public async Task UploadFile(string filename, string contentType, Stream content)
        {
            await Task.CompletedTask;
        }
    }
}
