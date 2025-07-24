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

        public Task<IEnumerable<Ticket>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AnalystTicketsAnalysis>> GetAnalystTicketsAnalysis()
        {
            try
            {
                var analysis = fakeTickets.Where(x => x.AssignedTo != null).GroupBy(x => new { analystId = x.AssignedTo.AnalystId, analystName = x.AssignedTo.AnalystName })
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

        public Task<IEnumerable<Interaction>> GetInteractionsByTicketIdAsync(Guid ticketId)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<Ticket>> GetTicketsByAnalystIdAsync(string analystId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketsByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketsByPriorityAsync(EPriority priority)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
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
                                            analystName = x.AssignedTo.AnalystName
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
