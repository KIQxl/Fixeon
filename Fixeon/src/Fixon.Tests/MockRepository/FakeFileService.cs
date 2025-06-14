using Fixeon.Domain.Application.Dtos;
using Fixeon.Domain.Application.Interfaces;

namespace Fixon.Tests.MockRepository
{
    public class FakeFileService : IFileServices
    {
        public Task SaveFile(FormFileAdapterDto file)
        {
            return Task.CompletedTask;
        }
    }
}
