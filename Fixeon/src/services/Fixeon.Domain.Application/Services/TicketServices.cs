using Fixeon.Domain.Application.Configurations;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.Interfaces;
using Fixeon.Domain.Core.ValueObjects;

namespace Fixeon.Domain.Application.Services
{
    public class TicketServices : ITicketServices
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketServices(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Response<TicketResponse>> CreateTicket(CreateTicketRequest request)
        {
            var validationResult = request.Validate();

            if (!validationResult.IsValid)
                return new Response<TicketResponse>(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), EErrorType.BadRequest);

            var ticket = new Ticket(
                request.Title,
                request.Description,
                request.Category,
                new User { UserId = request.CreateByUserId, UserName = request.CreateByUsername },
                request.Priority,
                new Attachment { FirstAttachment = request.FirstAttachment, SecondAttachment = request.SecondAttachment, ThirdAttachment = request.ThirdAttachment });

            try
            {
                var resultDB = await _ticketRepository.CreateTicket(ticket);

                if (!resultDB)
                    return new Response<TicketResponse>("Não foi possível realizar a abertura do chamado.", EErrorType.ServerError);

                return new Response<TicketResponse>(ticket.ToResponse());
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<TicketResponse>> CreateTicketInteraction(CreateInteractionRequest request)
        {
            var validationResult = request.Validate();

            if (!validationResult.IsValid)
                return new Response<TicketResponse>(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), EErrorType.BadRequest);

            var ticket = await _ticketRepository.GetTicketByIdAsync(request.TicketId);

            if (ticket is null)
                return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

            var interaction = new Interaction(request.TicketId, request.Message, new InteractionUser { UserId = request.CreatedByUserId, UserName = request.CreatedByUserName });

            if(!ticket.NewInteraction(interaction))
                return new Response<TicketResponse>($"O ticket {ticket.Id} está cancelado. Tickets cancelados não podem ser modificados. Solicite a reabertura do ticket para realizar modificações.", EErrorType.BadRequest);

            try
            {
                await _ticketRepository.CreateInteraction(interaction);
                var resultDB = await _ticketRepository.UpdateTicket(ticket);
                if (resultDB)
                {
                    return new Response<TicketResponse>(ticket.ToResponse());
                }

                return new Response<TicketResponse>($"Não foi possível adicionar a interação ao ticket: {ticket.Id}.", EErrorType.ServerError);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }
            
        public async Task<Response<TicketResponse>> GetTicketByIdAsync(Guid id)
        {
            try
            {
                var ticket = await _ticketRepository.GetTicketByIdAsync(id);

                if (ticket is null)
                    return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

                return new Response<TicketResponse>(ticket.ToResponse());
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<TicketResponse>>> GetTicketsByPriorityAsync(EPriority priority)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketsByPriorityAsync(priority);

                if (tickets is null)
                    return new Response<IEnumerable<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(x => x.ToResponse());

                return new Response<IEnumerable<TicketResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<IEnumerable<TicketResponse>>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<TicketResponse>>> GetTicketsByCategoryAsync(string category)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketsByCategoryAsync(category);

                if (tickets is null)
                    return new Response<IEnumerable<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(x => x.ToResponse());

                return new Response<IEnumerable<TicketResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<IEnumerable<TicketResponse>>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsAsync()
        {
            try
            {
                var tickets = await _ticketRepository.GetAllTicketsAsync();

                if (tickets is null)
                    return new Response<IEnumerable<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(x => x.ToResponse());

                return new Response<IEnumerable<TicketResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<IEnumerable<TicketResponse>>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<TicketResponse>>> GetTicketsByUserIdAsync(string userId)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketsByUserIdAsync(userId);

                if (tickets is null)
                    return new Response<IEnumerable<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(x => x.ToResponse());

                return new Response<IEnumerable<TicketResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<IEnumerable<TicketResponse>>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<TicketResponse>>> GetTicketsByAnalistIdAsync(string analistId)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketsByAnalistIdAsync(analistId);

                if (tickets is null)
                    return new Response<IEnumerable<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(x => x.ToResponse());

                return new Response<IEnumerable<TicketResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<IEnumerable<TicketResponse>>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<InteractionResponse>>> GetInteractionsByTicketIdAsync(Guid ticketId)
        {
            try
            {
                var interactions = await _ticketRepository.GetInteractionsByTicketIdAsync(ticketId);

                if (interactions is null)
                    return new Response<IEnumerable<InteractionResponse>>("Interações não encontradas.", EErrorType.NotFound);

                var responses = interactions.Select(x => x.ToInteractionResponse());

                return new Response<IEnumerable<InteractionResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<IEnumerable<InteractionResponse>>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<TicketResponse>> AssignTicketTo(CreateAssignTicketRequest request)
        {
            try
            {
                var ticket = await _ticketRepository.GetTicketByIdAsync(request.TicketId);

                if (ticket is null)
                    return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

                if(!ticket.AssignTicketToAnalist(new Analist { AnalistId = request.AnalistId, AnalistName = request.AnalistName }))
                    return new Response<TicketResponse>($"O ticket {ticket.Id} está cancelado. Tickets cancelados não podem ser modificados. Solicite a reabertura do ticket para realizar modificações.", EErrorType.BadRequest);

                var resultDB = await _ticketRepository.UpdateTicket(ticket);

                if (resultDB)
                    return new Response<TicketResponse>(ticket.ToResponse());

                return new Response<TicketResponse>($"Não foi possível atribuir o ticket {ticket.Id} ao Analista {request.AnalistName} - {request.AnalistId}.", EErrorType.ServerError);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<TicketResponse>> ChangeTicketStatus(ChangeTicketStatusRequest request)
        {
            try
            {
                var ticket = await _ticketRepository.GetTicketByIdAsync(request.TicketId);

                if (ticket is null)
                    return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

                switch (request.Status)
                {
                    case ETicketStatus.Resolved:
                        ticket.ResolveTicket();
                        break;

                    case ETicketStatus.Canceled:
                        ticket.CancelTicket();
                        break;

                    case ETicketStatus.Reopened:
                        if (!ticket.ReOpenTicket())
                            return new Response<TicketResponse>("Tickets cancelados não podem ser reabertos.", EErrorType.BadRequest);
                        break;

                    default:
                        return new Response<TicketResponse>("Status inválido.", EErrorType.BadRequest);
                }

                await _ticketRepository.UpdateTicket(ticket);

                return new Response<TicketResponse>(ticket.ToResponse());
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }
    }
}
