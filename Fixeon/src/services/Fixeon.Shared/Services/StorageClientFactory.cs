using Fixeon.Shared.Configuration;
using Fixeon.Shared.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace Fixeon.Shared.Services
{
    public class StorageClientFactory
    {
        private readonly StorageSettings _storageSettings;
        private readonly ITenantContextServices _tenantContext;

        public StorageClientFactory(IOptions<StorageSettings> storageSettings, ITenantContextServices tenantContext)
        {
            _storageSettings = storageSettings.Value;
            _tenantContext = tenantContext;
        }

        public StorageService GetService()
        {
            return (_storageSettings.ProviderName switch
            {
                "S3" => new S3StorageService(_storageSettings, _tenantContext),
                "MinIO" => new MinIOStorageService(_storageSettings, _tenantContext)
            });
        }
    }
}