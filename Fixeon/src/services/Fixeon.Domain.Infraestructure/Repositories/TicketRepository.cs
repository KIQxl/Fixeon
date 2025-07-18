﻿using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Infraestructure.Data;
using Fixeon.Shared.Interfaces;
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
                return await _ctx.interactions.AsNoTracking().Where(i => i.TicketId.Equals(ticketId)).ToListAsync();
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
                return await _ctx.tickets.Include(i => i.Interactions).FirstOrDefaultAsync(t => t.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByAnalistIdAsync(string analistId)
        {
            try
            {
                return await _ctx.tickets.AsNoTracking().Where(t => t.AssignedTo.AnalistId.Equals(analistId)).Include(i => i.Interactions).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByCategoryAsync(string category)
        {
            try
            {
                return await _ctx.tickets.AsNoTracking().Where(t => t.Category.ToUpper().Equals(category.ToUpper())).Include(i => i.Interactions).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByPriorityAsync(EPriority priority)
        {
            try
            {
                return await _ctx.tickets.AsNoTracking().Where(t => t.Priority.Equals(priority)).Include(i => i.Interactions).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao acessar a base de dados: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            try
            {
                return await _ctx.tickets.AsNoTracking().Where(t => t.CreatedByUser.UserId.Equals(userId)).Include(i => i.Interactions).ToListAsync();
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
    }
}
