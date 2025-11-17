namespace Fixeon.Shared.Core.Interfaces
{
    public interface IStorageServices
    {
        public Task UploadFile(string folder, string filename, string contentType, Stream content);
        public Task<string> GetPresignedUrl(string filename);
    }
}
