using Fixeon.Domain.Core.Entities;

namespace Fixeon.Domain.Application.Dtos
{
    public class FormFileAdapterDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public Stream Content { get; set; }

        public Attachment ToAttachment(Guid sender, Guid? ticketId, Guid? interactionId)
        {
            return new Attachment(FileName, Path.GetExtension(FileName), sender, ticketId, interactionId);
        }
    }
}
