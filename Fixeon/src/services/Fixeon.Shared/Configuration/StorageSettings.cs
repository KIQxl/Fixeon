namespace Fixeon.Shared.Configuration
{
    public class StorageSettings
    {
        public string ProviderName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string EndPoint { get; set; }
        public bool UseSSL { get; set; }
        public bool ForcePathStyle { get; set; }
        public string BucketName { get; set; }
    }
}
