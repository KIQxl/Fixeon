namespace Fixeon.Domain.Application.Dtos.Responses
{
    public record TopAnalystResponse
    {
        public string AnalystName { get; set; }
        public int TicketsLast30Days { get; set; }
        public string AverageTime { get; set; }
    }
}
