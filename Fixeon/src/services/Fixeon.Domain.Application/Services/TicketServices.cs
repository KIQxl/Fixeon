using Fixeon.Domain.Application.Configurations;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;
using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;

namespace Fixeon.Domain.Application.Services
{
    public class TicketServices : ITicketServices
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageServices _storageServices;
        private readonly IBackgroundEmailJobWrapper _backgroundServices;
        private readonly ITenantContext _tenantContext;
        public TicketServices(ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IStorageServices storageServices, IBackgroundEmailJobWrapper backgroundServices, ITenantContext tenantContext)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _storageServices = storageServices;
            _backgroundServices = backgroundServices;
            _tenantContext = tenantContext;
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
                request.Departament,
                request.Priority.ToString(),
                new User { UserId = _tenantContext.UserId.ToString(), UserEmail = _tenantContext.UserEmail, OrganizationId = _tenantContext.OrganizationId, OrganizationName = _tenantContext.OrganizationName });

            foreach (var file in request.Attachments)
            {
                await _storageServices.UploadFile(file.FileName, file.ContentType, file.Content);
                var attachment = file.ToAttachment(_tenantContext.UserId, ticket.Id, null);
                ticket.AddAttachment(attachment);
            }

            try
            {
                await _ticketRepository.CreateTicket(ticket);

                var result = await _unitOfWork.Commit();

                if (!result)
                    return new Response<TicketResponse>("Não foi possível realizar a abertura do chamado.", EErrorType.ServerError);

                _backgroundServices.SendEmail(new EmailMessage
                {
                    To = _tenantContext.UserEmail,
                    Subject = "Novo ticket aberto",
                    Body = EmailDictionary.NewTicketInformAnalysts
                    .Replace("{ticketId}", ticket.Id.ToString())
                    .Replace("{ticketUser}", ticket.CreatedByUser.UserEmail)
                    .Replace("{ticketTitle}", ticket.Title)
                    .Replace("{ticketCreatedAt}", ticket.CreateAt.ToString("dd/MM/yyyy HH:mm"))
                });
                _backgroundServices.SendEmail(new EmailMessage { To = _tenantContext.UserEmail, Subject = "Ticket registrado com sucesso!", Body = EmailDictionary.ConfirmationTicketOpening });

                return new Response<TicketResponse>(ticket.ToResponse());
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<TicketResponse>> ChangeTicketCategory(ChangeTicketCategory request)
        {
            try
            {
                var ticket = await _ticketRepository.GetTicketByIdAsync(request.Id);
                if (ticket is null)
                    return new Response<TicketResponse>("Ticket não encontrado", EErrorType.NotFound);

                ticket.ChangeCategory(request.Category);

                await _ticketRepository.UpdateTicket(ticket);

                return new Response<TicketResponse>(ticket.ToResponse());
            }
            catch (Exception ex)
            {
                return new Response<TicketResponse>(ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<TicketResponse>> CreateTicketInteraction(CreateInteractionRequest request)
        {
            var validationResult = request.Validate();

            if (!validationResult.IsValid)
                return new Response<TicketResponse>(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), EErrorType.BadRequest);

            var ticket = await _ticketRepository.GetTicketByIdAsync(request.TicketId);

            if(ticket.Status != ETicketStatus.InProgress.ToString() || ticket.Status != ETicketStatus.Reopened.ToString())
                return new Response<TicketResponse>("Ticket finalizado. não foi possível adicionar a interação.", EErrorType.NotFound);

            if (ticket is null)
                return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

            if (ticket.Status.Equals(ETicketStatus.Canceled))
                return new Response<TicketResponse>($"O ticket {ticket.Id} está cancelado. Tickets cancelados não podem ser modificados. Solicite a reabertura do ticket para realizar modificações.", EErrorType.BadRequest);

            var interaction = new Interaction(request.TicketId, request.Message, new InteractionUser { UserId = _tenantContext.UserId.ToString(), UserEmail = _tenantContext.UserEmail});

            ticket.NewInteraction(interaction);

            foreach (var file in request.Attachments)
            {
                await _storageServices.UploadFile(file.FileName, file.ContentType, file.Content);

                var attachment = file.ToAttachment(_tenantContext.UserId, null, interaction.Id);
                interaction.AddAttachment(attachment);
            }

            try
            {
                await _ticketRepository.CreateInteraction(interaction);
                await _ticketRepository.UpdateTicket(ticket);

                var result = await _unitOfWork.Commit();

                if (result)
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

                var attachmentsUrl = await GetAttachmentsUrl(ticket.Attachments);
                var response = new Response<TicketResponse>(ticket.ToResponse(attachmentsUrl));

                var interactions = ticket.Interactions.Select(i =>
                {
                    var interactionsAttachments = GetAttachmentsUrl(i.Attachments);
                    return i.ToInteractionResponse(interactionsAttachments.Result);

                }).ToList();

                response.Data.AddInteractionsResponseForTicket(interactions);

                return response;
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsAsync()
        {
            try
            {
                var tickets = await _ticketRepository.GetAllTicketsAsync();

                if (tickets is null)
                    return new Response<IEnumerable<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(x =>
                {
                    var urls = GetAttachmentsUrl(x.Attachments);
                    return x.ToResponse(urls.Result);
                });

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

                var responses = interactions.Select(x =>
                {
                    var urls = GetAttachmentsUrl(x.Attachments);
                    return x.ToInteractionResponse(urls.Result);
                });

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

                if (ticket.AssignedTo == new Analyst { AnalystId = request.AnalystId, AnalystEmail = request.AnalystEmail })
                    return new Response<TicketResponse>("Esse ticket já pertence a esse analista.", EErrorType.BadRequest);

                if (!ticket.AssignTicketToAnalyst(new Analyst { AnalystId = request.AnalystId, AnalystEmail = request.AnalystEmail }))
                    return new Response<TicketResponse>($"Esse ticket está cancelado ou finalizado. Tickets cancelados ou finalizados não podem ser modificados. Solicite a reabertura do ticket para realizar modificações.", EErrorType.BadRequest);

                await _ticketRepository.UpdateTicket(ticket);

                var result = await _unitOfWork.Commit();

                if (result)
                    return new Response<TicketResponse>(ticket.ToResponse());

                return new Response<TicketResponse>($"Não foi possível atribuir o ticket {ticket.Id} ao Analista {request.AnalystEmail} - {request.AnalystId}.", EErrorType.ServerError);
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

                if(ticket.Status.Equals(request.Status.ToString()))
                    return new Response<TicketResponse>("Essa ação já foi realizada.", EErrorType.BadRequest);

                if (ticket is null)
                    return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

                switch (request.Status)
                {
                    case ETicketStatus.Resolved:
                        if(!ticket.ResolveTicket(new Analyst { AnalystId = _tenantContext.UserId.ToString(), AnalystEmail = _tenantContext.UserEmail }))
                            return new Response<TicketResponse>("Esse ticket não pode ser fechado pois já foi finalizado, cancelado ou não possui um analista responsável.", EErrorType.BadRequest);
                        break;

                    case ETicketStatus.Canceled:
                        ticket.CancelTicket();
                        break;

                    case ETicketStatus.Reopened:
                        if (!ticket.ReOpenTicket())
                            return new Response<TicketResponse>("Esse ticket não pode ser reaberto pois ainda não foi finalizado ou está cancelado.", EErrorType.BadRequest);
                        break;

                    default:
                        return new Response<TicketResponse>("Status inválido.", EErrorType.BadRequest);
                }

                await _ticketRepository.UpdateTicket(ticket);
                var result = await _unitOfWork.Commit();

                if (!result)
                    return new Response<TicketResponse>("Não foi possível realizar a operação.", EErrorType.ServerError);

                return new Response<TicketResponse>(ticket.ToResponse());
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst, Guid? user)
        {
            try
            {
                var tickets = await _ticketRepository.GetAllTicketsFilterAsync(category, status, priority, analyst, user);

                if (tickets is null)
                    return new Response<IEnumerable<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(t =>
                {
                    var ticketAttachmentsUrls = GetAttachmentsUrl(t.Attachments);
                    return t.ToResponse(ticketAttachmentsUrls.Result);
                })
                .ToList();

                return new Response<IEnumerable<TicketResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<IEnumerable<TicketResponse>>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<TicketDashboardResponse>> GetDashboardTickets()
        {
            try
            {
                var ticketAnalysis = await _ticketRepository.GetTicketsAnalysis();

                var analystTicketAnalysis = await _ticketRepository.GetAnalystTicketsAnalysis();

                var topAnalystAnalysis = await _ticketRepository.GetTopAnalyst();

                var result = new TicketDashboardResponse
                {
                    TicketAnalysisResponse = ticketAnalysis,
                    analystTicketsAnalyses = analystTicketAnalysis,
                    TopAnalystResponse = topAnalystAnalysis
                };

                return new Response<TicketDashboardResponse>(result);
            }
            catch (Exception ex)
            {
                return new Response<TicketDashboardResponse>(ex.InnerException?.Message ?? ex.Message, EErrorType.ServerError);
            }
        }

        private async Task<List<string>> GetAttachmentsUrl(List<Attachment> attachments)
        {
            var urls = new List<string>();

            foreach (var attachment in attachments)
            {
                var presignedUrl = await _storageServices.GetPresignedUrl(attachment.Name);
                urls.Add(presignedUrl);
            }

            return urls;
        }
    }
}
