using Fixeon.Domain.Application.Dtos;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Core.Enums;

namespace Fixeon.WebApi.Dtos.Requests
{
    public class CreateInteractionRequestDto
    {
        public Guid TicketId { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public string Message { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

        public CreateInteractionRequest ToApplicationRequest()
        {
            var attachments = Files.Select(file => new FormFileAdapterDto
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Length = file.Length,
                Content = file.OpenReadStream()
            }).ToList();

            var request = new CreateInteractionRequest
            {
                TicketId = TicketId,
                CreatedByUserId = CreatedByUserId,
                CreatedByUserName = CreatedByUserName,
                Message = Message,
                Attachments = attachments
            };

            return request;
        }
    }
}
