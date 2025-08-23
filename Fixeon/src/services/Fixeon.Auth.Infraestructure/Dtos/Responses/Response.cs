using Fixeon.Auth.Infraestructure.Interfaces;

namespace Fixeon.Auth.Application.Dtos.Responses
{
    public class Response<T> : IResponse
    {
        public Response(T data, bool? success = true)
        {
            Data = data;
            Success = true;
        }

        public Response(string error)
        {
            Data = default;
            Success = false;
            Errors.Add(error);
        }

        public Response(List<string> errors)
        {
            Data = default;
            Success = false;
            Errors = errors;
        }

        public T Data { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
