namespace Fixeon.Shared.Core.Interfaces
{
    public interface IStorageServices
    {
        public Task UploadFile(string filename, string contentType, Stream content);
    }
}
