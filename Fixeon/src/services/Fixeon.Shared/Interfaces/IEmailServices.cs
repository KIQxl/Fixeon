namespace Fixeon.Shared.Interfaces
{
    public interface IEmailServices
    {
        public Task SendEmail(string to, string subject, string body);
    }
}
