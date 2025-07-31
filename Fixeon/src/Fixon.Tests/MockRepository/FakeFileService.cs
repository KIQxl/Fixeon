using Fixeon.Domain.Application.Dtos;

namespace Fixon.Tests.MockRepository
{
    public class FakeFileService
    {
        public Task SaveFile(FormFileAdapterDto file)
        {
            return Task.CompletedTask;
        }
    }
}
