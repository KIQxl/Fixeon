namespace Fixeon.Shared.Configuration
{
    public class MinIOStorageSettings
    {
        public string EndPoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public bool UseSSL { get; set; }
        public bool ForcePathStyle { get; set; }
    }
}
