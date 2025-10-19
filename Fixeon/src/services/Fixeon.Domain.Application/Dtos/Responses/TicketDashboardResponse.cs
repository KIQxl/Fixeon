namespace Fixeon.Domain.Application.Dtos.Responses
{
    public record TicketDashboardResponse
    {
        public TicketAnalysisResponse TicketAnalysisResponse { get; set; }
        public List<AnalystTicketsAnalysis> analystTicketsAnalyses { get; set; } = new List<AnalystTicketsAnalysis>();
        public List<TopAnalystResponse> TopAnalystResponse { get; set; }
        public List<TicketsByDayResponse> TicketsByDay { get; set; }
        public List<TicketsByHourResponse> TicketsByHour { get; set; }
    }
}
