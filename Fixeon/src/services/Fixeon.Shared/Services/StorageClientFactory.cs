using Fixeon.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace Fixeon.Shared.Services
{
    public class StorageClientFactory
    {
        private readonly StorageSettings _storageSettings;

        public StorageClientFactory(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings.Value;
        }

        public StorageService GetService()
        {
            return (_storageSettings.ProviderName switch
            {
                "S3" => new S3StorageService(_storageSettings),
                "MinIO" => new MinIOStorageService(_storageSettings)
            });
        }
    }
}
