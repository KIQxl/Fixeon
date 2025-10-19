namespace Fixeon.Domain.Application.Dtos.Responses
{
    public class TicketsByDayResponse
    {
        public DateTime Date { get; set; }
        public int Created { get; set; }
        public int Resolved { get; set; }
    }
}
