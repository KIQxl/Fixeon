using Fixeon.Domain.Application.Dtos;

namespace Fixeon.Domain.Application.Interfaces
{
    public interface IFileServices
    {
        public Task SaveFile(FormFileAdapterDto file);
    }
}
