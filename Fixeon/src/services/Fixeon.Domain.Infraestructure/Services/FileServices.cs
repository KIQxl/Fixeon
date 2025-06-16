using Fixeon.Domain.Application.Dtos;
using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Infraestructure.Services
{
    public class FileServices : IFileServices
    {
        public Task SaveFile(FormFileAdapterDto file)
        {
            return Task.CompletedTask;
        }
    }
}
