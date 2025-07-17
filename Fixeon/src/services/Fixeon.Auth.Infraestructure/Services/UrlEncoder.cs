using Fixeon.Auth.Infraestructure.Interfaces;
using System.Web;

namespace Fixeon.Auth.Infraestructure.Services
{
    public class UrlEncoder : IUrlEncoder
    {
        public string Encode(string value) => HttpUtility.UrlEncode(value);
        public string Decode(string value) => HttpUtility.UrlDecode(value);
    }
}
