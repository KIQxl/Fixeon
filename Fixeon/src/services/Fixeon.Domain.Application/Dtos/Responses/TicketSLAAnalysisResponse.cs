namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class TicketSLAAnalysisResponse
    {
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        public int TotalTickets { get; set; }

        public int ResolutionTicketsWithSLA { get; set; }
        public int ResolutionWithinSLA { get; set; }
        public double ResolutionWithinSLAPercentage { get; set; }

        public int FirstInteractionTicketsWithSLA { get; set; }
        public int FirstInteractionWithinSLA { get; set; }
        public double FirstInteractionWithinSLAPercentage { get; set; }
    }

}
