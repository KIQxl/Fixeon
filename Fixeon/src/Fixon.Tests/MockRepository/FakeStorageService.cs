using Fixeon.Shared.Core.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeStorageService : IStorageServices
    {
        public Task UploadFile(string filename, string contentType, Stream content)
        {
            throw new NotImplementedException();
        }
    }
}
