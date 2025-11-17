namespace Fixeon.Shared.Core.Models
{
    public class FormFileAdapterDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public Stream Content { get; set; }

        public string GetExtension()
        {
            return Path.GetExtension(this.FileName);
        }
    }
}
