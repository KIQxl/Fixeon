using System.Text.Json.Serialization;

namespace Fixeon.Domain.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EActiveStatus
    {
        Active,
        Inactive,
        Trial,
        Suspended,
        Onboarding
    }
}
