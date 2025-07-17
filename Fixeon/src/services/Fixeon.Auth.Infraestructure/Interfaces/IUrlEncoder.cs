using System.Web;

namespace Fixeon.Auth.Infraestructure.Interfaces
{
    public interface IUrlEncoder
    {
        public string Encode(string value);
        public string Decode(string value);
    }
}
