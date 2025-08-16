using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Domain.Infraestructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DomainContext _ctx;

        public TicketRepository(DomainContext ctx)
        {
            _ctx = ctx;
        }

        public async Task CreateInteraction(Interaction interaction)
        {
            try
            {
                await _ctx.interactions.AddAsync(interaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task CreateTicket(Ticket ticket)
        {
            try
            {
                await _ctx.tickets.AddAsync(ticket);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                return await _ctx.tickets.AsNoTracking().Include(t => t.Interactions).Include(t => t.Attachments).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Interaction>> GetInteractionsByTicketIdAsync(Guid ticketId)
        {
            try
            {
                return await _ctx.interactions.AsNoTracking().Where(i => i.TicketId.Equals(ticketId)).Include(i => i.Attachments).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<Ticket> GetTicketByIdAsync(Guid id)
        {
            try
            {
                return await _ctx.tickets.Include(i => i.Interactions).ThenInclude(i => i.Attachments).Include(a => a.Attachments).FirstOrDefaultAsync(t => t.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task UpdateTicket(Ticket ticket)
        {
            try
            {
                _ctx.Attach(ticket);
                _ctx.Entry(ticket).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task CreateAttachment(List<Attachment> attachments)
        {
            try
            {
                await _ctx.attachments.AddRangeAsync(attachments);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst, Guid? user, string? protocol)
        {
            try
            {
                var query = _ctx.tickets.AsQueryable();

                if (!string.IsNullOrEmpty(category))
                    query = query.Where(t => t.Category == category);

                if (!string.IsNullOrEmpty(status))
                    query = query.Where(t => t.Status == status);

                if (!string.IsNullOrEmpty(priority))
                    query = query.Where(t => t.Priority == priority);

                if (analyst.HasValue)
                    query = query.Where(t => t.AssignedTo.AnalystId == analyst.ToString());

                if (user.HasValue)
                    query = query.Where(t => t.CreatedByUser.UserId == user.ToString());

                if (!string.IsNullOrEmpty(protocol))
                    query = query.Where(t => t.Protocol == protocol);

                return await query.Include(i => i.Interactions).Include(a => a.Attachments).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<TicketAnalysisResponse> GetTicketsAnalysis()
        {
            try
            {
                var analysis = await _ctx.tickets.GroupBy(x => 1)
                    .Select(x => new TicketAnalysisResponse
                    {
                        Pending = x.Count(t => t.Status == ETicketStatus.Pending.ToString()),
                        InProgress = x.Count(t => t.Status == ETicketStatus.InProgress.ToString()),
                        Resolved = x.Count(t => t.Status == ETicketStatus.Resolved.ToString()),
                        Canceled = x.Count(t => t.Status == ETicketStatus.Canceled.ToString()),
                        ReOpened = x.Count(t => t.Status == ETicketStatus.Reopened.ToString()),
                        Total = x.Count()
                    }
                ).FirstOrDefaultAsync() ?? new TicketAnalysisResponse();

                return analysis;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AnalystTicketsAnalysis>> GetAnalystTicketsAnalysis()
        {
            try
            {
                var analysis = _ctx.tickets
                                .Where(x => x.AssignedTo != null)
                                .Select(t => new
                                {
                                    t.AssignedTo.AnalystId,
                                    t.AssignedTo.AnalystEmail,
                                    t.Status,
                                    t.CreateAt,
                                    t.ResolvedAt
                                })
                                .AsEnumerable()
                                .GroupBy(x => new { x.AnalystId, x.AnalystEmail })
                                .Select(g => new AnalystTicketsAnalysis
                                {
                                    AnalystId = g.Key.AnalystId,
                                    AnalystName = g.Key.AnalystEmail,
                                    PendingTickets = g.Count(t => t.Status == ETicketStatus.Pending.ToString()),
                                    ResolvedTickets = g.Count(t => t.Status == ETicketStatus.Resolved.ToString()),
                                    TicketsTotal = g.Count(t => t.AnalystId == g.Key.AnalystId),
                                    AverageResolutionTimeInHours = ConvertInHours(
                                        g.Where(t => t.ResolvedAt.HasValue)
                                         .Select(t => (t.ResolvedAt.Value - t.CreateAt).TotalHours)
                                         .DefaultIfEmpty(0)
                                         .Average()
                                    )
                                })
                                .ToList() ?? new List<AnalystTicketsAnalysis>();

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
                var analysis = _ctx.tickets
                                        .Where(x => x.AssignedTo != null)
                                        .Select(t => new
                                        {
                                            t.AssignedTo.AnalystEmail,
                                            t.AssignedTo.AnalystId,
                                            t.Status,
                                            t.CreateAt,
                                            t.ResolvedAt
                                        })
                                        .AsEnumerable()
                                        .GroupBy(x => new
                                        {
                                            analystId = x.AnalystId,
                                            analystName = x.AnalystEmail
                                        })
                                        .Select(x => new TopAnalystResponse
                                        {
                                            AnalystName = x.Key.analystName,
                                            TicketsLast30Days = x.Count(t => t.CreateAt >= DateTime.Now.AddDays(-30) && t.Status == ETicketStatus.Resolved.ToString()),
                                            AverageTime = ConvertInHours(x.Where(t => t.ResolvedAt.HasValue)
                                                            .Select(t => (t.ResolvedAt.Value - t.CreateAt).TotalHours)
                                                            .DefaultIfEmpty(0)
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

        private string ConvertInHours(double average)
        {
            var avgTime = TimeSpan.FromHours(average);

            return $"{(int)avgTime.TotalHours}h{(int)avgTime.TotalMinutes}m";
        }
    }
}
