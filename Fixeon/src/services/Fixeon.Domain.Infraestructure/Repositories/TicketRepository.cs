using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Infraestructure.Data;
using Microsoft.Data.SqlClient;
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
        
        // TICKETS
        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(Guid? userId)
        {
            try
            {
                var query = _ctx.tickets.AsNoTracking().Where(x => x.Status != ETicketStatus.Resolved.ToString() && x.Status != ETicketStatus.Canceled.ToString()).AsQueryable();

                if (userId.HasValue)
                    query = query.Where(x => x.CreatedByUser.UserId == userId.ToString());

                return await query.Include(t => t.Interactions).Include(t => t.Attachments).ToListAsync();
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

        public async Task<Ticket> GetTicketByIdAsync(Guid id)
        {
            try
            {
                return await _ctx.tickets.Include(i => i.Interactions).ThenInclude(i => i.Attachments).Include(a => a.Attachments).Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id.Equals(id));
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

        public async Task AttachTagInTicket(Guid ticketId, Guid tagId)
        {
            var sql = "INSERT INTO TicketTags (TicketId, TagId) VALUES (@ticketId, @tagId)";
            await _ctx.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@ticketId", ticketId),
                new SqlParameter("@tagId", tagId));
        }

        public async Task DetachTagInTicket(Guid ticketId, Guid tagId)
        {
            var sql = "DELETE FROM TicketTags WHERE TicketId = @ticketId AND TagId = @tagId";
            await _ctx.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@ticketId", ticketId),
                new SqlParameter("@tagId", tagId));
        }


        // INTERACTIONS
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

        //ATTACHMENTS
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

        public async Task<TicketAnalysisResponse> GetTicketsAnalysis()
        {
            try
            {
                var analysis = _ctx.tickets
                    .Select(x => new
                    {
                        x.Status,
                        x.ResolvedAt,
                        x.CreateAt
                    })
                    .AsEnumerable()
                    .GroupBy(x => 1)
                    .Select(x => new TicketAnalysisResponse
                    {
                        Pending = x.Count(t => t.Status == ETicketStatus.Pending.ToString()),
                        InProgress = x.Count(t => t.Status == ETicketStatus.InProgress.ToString()),
                        Resolved = x.Count(t => t.Status == ETicketStatus.Resolved.ToString()),
                        Canceled = x.Count(t => t.Status == ETicketStatus.Canceled.ToString()),
                        ReOpened = x.Count(t => t.Status == ETicketStatus.Reopened.ToString()),
                        Total = x.Count(),
                        AverageResolutionTimeInHours = ConvertInHours(
                                        x.Where(t => t.ResolvedAt.HasValue)
                                         .Select(t => (t.ResolvedAt.Value - t.CreateAt).TotalHours)
                                         .DefaultIfEmpty(0)
                                         .Average())
                    }).FirstOrDefault() ?? new TicketAnalysisResponse();

                return analysis;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TicketSLAAnalysisResponse>> GetTicketsSLAAnalysisByOrganizationAsync()
        {
            try
            {
                var tickets = await _ctx.tickets.Include(t => t.SLAInfo)
                    .AsNoTracking()
                    .Select(t => new
                    {
                        t.CreatedByUser.OrganizationId,
                        t.CreatedByUser.OrganizationName,
                        HasResolutionDeadline = t.SLAInfo != null && t.SLAInfo.Resolution.Deadline.HasValue,
                        ResolutionWithinDeadline = t.SLAInfo != null ? t.SLAInfo.Resolution.WithinDeadline : null as bool?,
                        HasFirstInteractionDeadline = t.SLAInfo != null && t.SLAInfo.FirstInteraction.Deadline.HasValue,
                        FirstInteractionWithinDeadline = t.SLAInfo != null ? t.SLAInfo.FirstInteraction.WithinDeadline : null as bool?
                    })
                    .ToListAsync();

                var grouped = tickets
                    .GroupBy(t => new { t.OrganizationId, t.OrganizationName })
                    .Select(g =>
                    {
                        var totalTickets = g.Count();

                        var resolutionWithSLA = g.Count(t => t.HasResolutionDeadline);
                        var resolutionWithin = g.Count(t => t.ResolutionWithinDeadline == true);
                        var resolutionPercentage = resolutionWithSLA == 0 ? 0 : Math.Round((double)resolutionWithin / resolutionWithSLA * 100, 2);

                        var firstInteractionWithSLA = g.Count(t => t.HasFirstInteractionDeadline);
                        var firstInteractionWithin = g.Count(t => t.FirstInteractionWithinDeadline == true);
                        var firstInteractionPercentage = firstInteractionWithSLA == 0 ? 0 : Math.Round((double)firstInteractionWithin / firstInteractionWithSLA * 100, 2);

                        return new TicketSLAAnalysisResponse
                        {
                            OrganizationId = g.Key.OrganizationId.Value,
                            OrganizationName = g.Key.OrganizationName,
                            TotalTickets = totalTickets,
                            ResolutionTicketsWithSLA = resolutionWithSLA,
                            ResolutionWithinSLA = resolutionWithin,
                            ResolutionWithinSLAPercentage = resolutionPercentage,
                            FirstInteractionTicketsWithSLA = firstInteractionWithSLA,
                            FirstInteractionWithinSLA = firstInteractionWithin,
                            FirstInteractionWithinSLAPercentage = firstInteractionPercentage
                        };
                    })
                    .ToList();

                return grouped;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter análise de SLA: {ex.Message}");
            }
        }


        public async Task<List<AnalystTicketsAnalysis>> GetAnalystTicketsAnalysis()
        {
            try
            {
                var analysis = _ctx.tickets
                                .Where(x => x.AssignedTo != null && x.Status == ETicketStatus.Resolved.ToString())
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

        public async Task<List<TicketsByDayResponse>> GetTicketsByDayAsync()
        {
            var startDate = DateTime.Now.Date.AddDays(-6);
            var endDate = DateTime.Now.Date.AddDays(1);

            var createdQuery = await _ctx.tickets
                .Where(t => t.CreateAt >= startDate && t.CreateAt < endDate)
                .GroupBy(t => t.CreateAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Created = g.Count()
                })
                .ToListAsync();

            var resolvedQuery = await _ctx.tickets
                .Where(t => t.ResolvedAt.HasValue && t.ResolvedAt.Value >= startDate && t.ResolvedAt.Value < endDate)
                .GroupBy(t => t.ResolvedAt.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Resolved = g.Count()
                })
                .ToListAsync();

            var result = Enumerable.Range(0, 7)
                .Select(i => startDate.AddDays(i))
                .Select(date => new TicketsByDayResponse
                {
                    Date = date,
                    Created = createdQuery.FirstOrDefault(c => c.Date == date)?.Created ?? 0,
                    Resolved = resolvedQuery.FirstOrDefault(r => r.Date == date)?.Resolved ?? 0
                })
                .OrderBy(x => x.Date)
                .ToList();

            return result;
        }

        public async Task<List<TicketsByHourResponse>> GetTicketsByHourAsync()
        {
            var startDate = DateTime.Now.Date.AddDays(-30);
            var endDate = DateTime.Now.Date.AddDays(1);

            var result = await _ctx.tickets
                .Where(t => t.CreateAt >= startDate && t.CreateAt < endDate)
                .GroupBy(t => t.CreateAt.Hour)
                .Select(g => new TicketsByHourResponse
                {
                    Hour = g.Key,
                    TicketsCreated = g.Count()
                })
                .OrderBy(x => x.Hour)
                .ToListAsync();

            var fullRange = Enumerable.Range(0, 24)
                .Select(hour => new TicketsByHourResponse
                {
                    Hour = hour,
                    TicketsCreated = result.FirstOrDefault(r => r.Hour == hour)?.TicketsCreated ?? 0
                })
                .ToList();

            return fullRange;
        }


        private string ConvertInHours(double average)
        {
            var avgTime = TimeSpan.FromHours(average);

            return $"{(int)avgTime.TotalHours}h{(int)avgTime.Minutes:D2}m";
        }
    }
}
