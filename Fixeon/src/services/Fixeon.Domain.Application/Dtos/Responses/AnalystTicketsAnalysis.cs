namespace Fixeon.Domain.Application.Dtos.Responses
{
    public record AnalystTicketsAnalysis
    {
        public string AnalystName { get; set; }
        public string AnalystId { get; set; }
        public int PendingTickets { get; set; }
        public int ResolvedTickets { get; set; }
        public int TicketsTotal { get; set; }
        public string AverageResolutionTimeInHours { get; set; }
    }
}
