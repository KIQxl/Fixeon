using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixon.Tests.MockRepository
{
    public class FakeTicketRepository : ITicketRepository
    {
        private readonly List<Ticket> fakeTickets = TicketsMock.GetTickets();
        private readonly List<Analyst> fakeAnalysts = TicketsMock.Analysts;
        private readonly List<Interaction> fakeInteractions = TicketsMock.Interactions();
        public Task CreateAttachment(List<Attachment> attachments)
        {
            return Task.CompletedTask;
        }

        public Task CreateInteraction(Interaction interaction)
        {
            return Task.CompletedTask;
        }

        public Task CreateTicket(Ticket ticket)
        {
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return fakeTickets;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst, Guid? user)
        {
            var mockTickets = fakeTickets.ToList();

            if (!string.IsNullOrEmpty(category))
                mockTickets = mockTickets.Where(x => x.Category == category).ToList();

            if (!string.IsNullOrEmpty(status))
                mockTickets = mockTickets.Where(x => x.Status == status).ToList();

            if (!string.IsNullOrEmpty(priority))
                mockTickets = mockTickets.Where(x => x.Priority == priority).ToList();

            if (analyst.HasValue)
                mockTickets = mockTickets.Where(x => x.AssignedTo.AnalystId == analyst.ToString()).ToList();

            if (user.HasValue)
                mockTickets = mockTickets.Where(x => x.CreatedByUser.UserId == user.ToString()).ToList();

            return fakeTickets;
        }

        public async Task<List<AnalystTicketsAnalysis>> GetAnalystTicketsAnalysis()
        {
            try
            {
                var analysis = fakeTickets.Where(x => x.AssignedTo != null).GroupBy(x => new { analystId = x.AssignedTo.AnalystId, analystName = x.AssignedTo.AnalystEmail })
                    .Select(x => new AnalystTicketsAnalysis
                    {
                        AnalystId = x.Key.analystId,
                        AnalystName = x.Key.analystName,
                        PendingTickets = x.Count(t => t.Status == ETicketStatus.Pending.ToString()),
                        ResolvedTickets = x.Count(t => t.Status == ETicketStatus.Resolved.ToString()),
                        TicketsTotal = x.Count(t => t.AssignedTo.AnalystId == x.Key.analystId),
                        AverageResolutionTimeInHours = ConvertInHours(x.Where(t => t.Duration.HasValue)
                                .Select(t => t.Duration.Value.TotalHours)
                                .DefaultIfEmpty(0)
                                .Average())
                    }).ToList() ?? new List<AnalystTicketsAnalysis>();

                return analysis;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Interaction>> GetInteractionsByTicketIdAsync(Guid ticketId)
        {
            return fakeInteractions.Where(x => x.TicketId == ticketId).ToList();
        }

        public async Task<Ticket> GetTicketByIdAsync(Guid id)
        {
            return fakeTickets.FirstOrDefault();
        }

        public async Task<TicketAnalysisResponse> GetTicketsAnalysis()
        {
            try
            {
                var analysis = fakeTickets.GroupBy(x => 1)
                    .Select(x => new TicketAnalysisResponse
                    {
                        Pending = x.Count(t => t.Status == ETicketStatus.Pending.ToString()),
                        InProgress = x.Count(t => t.Status == ETicketStatus.InProgress.ToString()),
                        Resolved = x.Count(t => t.Status == ETicketStatus.Resolved.ToString()),
                        Canceled = x.Count(t => t.Status == ETicketStatus.Canceled.ToString()),
                        ReOpened = x.Count(t => t.Status == ETicketStatus.Reopened.ToString()),
                        Total = x.Count()
                    }
                ).FirstOrDefault() ?? new TicketAnalysisResponse();

                return analysis;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TopAnalystResponse>> GetTopAnalyst()
        {
            try
            {
                var analysis = fakeTickets
                                        .Where(x => x.AssignedTo != null)
                                        .GroupBy(x => new
                                        {
                                            analystId = x.AssignedTo.AnalystId,
                                            analystName = x.AssignedTo.AnalystEmail
                                        })
                                        .Select(x => new TopAnalystResponse
                                        {
                                            AnalystName = x.Key.analystName,
                                            TicketsLast30Days = x.Count(t => t.CreateAt >= DateTime.Now.AddDays(-30) && t.Status == ETicketStatus.Resolved.ToString()),
                                            AverageTime = ConvertInHours(x.Where(t => t.Duration.HasValue)
                                                            .Select(t => t.Duration.Value.TotalHours)
                                                            .DefaultIfEmpty(0) // se vazio, retorna 0
                                                            .Average())
                                        })
                                        .OrderByDescending(x => x.TicketsLast30Days)
                                        .ThenBy(x => x.AverageTime)
                                        .Take(3)
                                        .ToList()
                                        ?? new List<TopAnalystResponse>();

                return analysis;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task UpdateTicket(Ticket ticket)
        {
            return Task.CompletedTask;
        }

        private string ConvertInHours(double average)
        {
            var avgTime = TimeSpan.FromHours(average);

            return $"{(int)avgTime.TotalHours}h{(int)avgTime.TotalMinutes}m";
        }
    }
}
