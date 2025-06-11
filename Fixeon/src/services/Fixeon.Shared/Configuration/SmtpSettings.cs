namespace Fixeon.Shared.Configuration
{
    public class SmtpSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SenderName { get; set; } = "Equipe Fixeon";
        public string SenderEmail { get; set; } = string.Empty;
    }
}
