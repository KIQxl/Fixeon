using Fixeon.Domain.Application.Interfaces;

namespace Fixeon.Domain.Application.Dtos.Requests
{
    public class CreateCategoryRequest : IRequest
    {
        public string CategoryName { get; set; }
    }
}
