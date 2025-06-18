namespace Fixeon.Auth.Application.Interfaces
{
    public interface IUrlEncoder
    {
        public string Encode(string value);
        public string Decode(string value);
    }
}
