using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Core.Enums;
using Fixeon.Shared.Core.Models;

namespace Fixeon.WebApi.Dtos.Requests
{
    public class  CreateTicketRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Departament { get; set; }
        public EPriority Priority { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

        public CreateTicketRequest ToApplicationRequest()
        {
            var attachments = Files.Select(file => new FormFileAdapterDto
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Length = file.Length,
                Content = file.OpenReadStream()
            }).ToList();

            var request = new CreateTicketRequest
            {
                Title = this.Title,
                Description = this.Description,
                Category = this.Category,
                Departament = this.Departament,
                Priority = this.Priority,
                Attachments = attachments
            };

            return request;
        }
    }
}
