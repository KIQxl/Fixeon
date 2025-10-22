namespace Fixeon.Domain.Application.Dtos.Responses
{
    public record TicketAnalysisResponse
    {
        public int Pending { get; set; }
        public int InProgress { get; set; }
        public int Resolved { get; set; }
        public int Canceled { get; set; }
        public int ReOpened { get; set; }
        public int Total { get; set; }
        public string AverageResolutionTimeInHours { get; set; }
    }
}
