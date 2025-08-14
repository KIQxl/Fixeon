namespace Fixeon.Domain.Core.ValueObjects
{
    public record InteractionUser
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
