using Fixeon.Domain.Application.Dtos.Enums;
using System.Text.Json.Serialization;

namespace Fixeon.Domain.Application.Dtos.Responses
{
    public record Response<T>
    {
        public Response(T data)
        {
            Data = data;
            Success = true;
            ErrorType = null;
        }

        public Response(List<string> errors, EErrorType errorType)
        {
            Errors = errors;
            Success = false;
            ErrorType = errorType;
        }

        public Response(string error, EErrorType errorType)
        {
            Errors.Add(error);
            Success = false;
            ErrorType = errorType;
        }

        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool Success { get; set; }

        [JsonIgnore]
        public EErrorType? ErrorType { get; set; }
    }
}
