using Fixeon.Domain.Application.Contracts;
using Fixeon.Domain.Application.Dtos;
using Fixeon.Domain.Application.Dtos.Enums;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Dtos.Responses;
using Fixeon.Domain.Application.Interfaces;
using Fixeon.Domain.Application.Mapper;
using Fixeon.Domain.Application.Validator;
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
        private readonly IAuthACL _authACL;
        public TicketServices(ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IStorageServices storageServices, IBackgroundEmailJobWrapper backgroundServices, ITenantContext tenantContext, IAuthACL authACL)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _storageServices = storageServices;
            _backgroundServices = backgroundServices;
            _tenantContext = tenantContext;
            _authACL = authACL;
        }

        public async Task<Response<TicketResponse>> CreateTicket(CreateTicketRequest request)
        {
            var validationResult = request.Validate();

            if (!validationResult.IsValid)
                return new Response<TicketResponse>(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), EErrorType.BadRequest);

            var ticket = TicketMapper.ToEntity(request, _tenantContext);

            await ProccessTicketAttachment(ticket, request.Attachments);

            try
            {
                await _ticketRepository.CreateTicket(ticket);

                var result = await _unitOfWork.Commit();

                if (!result)
                    return new Response<TicketResponse>("Não foi possível realizar a abertura do chamado.", EErrorType.ServerError);

                var companyEmail = await _authACL.GetCompanyEmail(_tenantContext.TenantId);

                _backgroundServices.SendEmail(new EmailMessage
                {
                    To = companyEmail,
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

            if (ticket is null)
                return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

            if (ticket.Status != ETicketStatus.InProgress.ToString() && ticket.Status != ETicketStatus.Reopened.ToString())
                return new Response<TicketResponse>("Ticket finalizado ou ainda não tem um analista responsável. não foi possível adicionar a interação.", EErrorType.NotFound);

            if (ticket.Status.Equals(ETicketStatus.Canceled))
                return new Response<TicketResponse>($"O ticket {ticket.Id} está cancelado. Tickets cancelados não podem ser modificados. Solicite a reabertura do ticket para realizar modificações.", EErrorType.BadRequest);

            var interaction = InteractionMapper.ToEntity(request, _tenantContext);

            ticket.NewInteraction(interaction);

            await ProccessInteractionAttachment(interaction, request.Attachments);

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

        public async Task<Response<IEnumerable<TicketResponse>>> GetAllTicketsFilterAsync(string? category, string? status, string? priority, Guid? analyst, Guid? user, string? protocol)
        {
            try
            {
                var tickets = await _ticketRepository.GetAllTicketsFilterAsync(category, status, priority, analyst, user, protocol);

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

        public async Task<Response<List<string>>> GetCategories()
        {
            try
            {
                var categories = await _ticketRepository.GetCategories();

                return new Response<List<string>>(categories.Select(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<string>>(ex.InnerException?.Message ?? ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<bool>> CreateCategory(CreateCategoryRequest request)
        {
            try
            {
                var validationResult = request.Validate();

                if (!validationResult.IsValid)
                    return new Response<bool>(validationResult.Errors.Select(e => e.ErrorMessage).ToList(), EErrorType.BadRequest);

                var category = new Category(request.CategoryName);

                await _ticketRepository.CreateCategory(category);

                var result = await _unitOfWork.Commit();
                if(!result)
                    return new Response<bool>("Não foi possivel cadastrar a categoria.", EErrorType.BadRequest);


                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex.Message ?? ex.InnerException.Message, EErrorType.BadRequest);
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

        private async Task ProccessTicketAttachment(Ticket ticket, List<FormFileAdapterDto> attachments)
        {
            foreach (var file in attachments)
            {
                await _storageServices.UploadFile(file.FileName, file.ContentType, file.Content);
                var attachment = file.ToAttachment(_tenantContext.UserId, ticket.Id, null);
                ticket.AddAttachment(attachment);
            }
        }

        private async Task ProccessInteractionAttachment(Interaction interaction, List<FormFileAdapterDto> attachments)
        {
            foreach (var file in attachments)
            {
                await _storageServices.UploadFile(file.FileName, file.ContentType, file.Content);

                var attachment = file.ToAttachment(_tenantContext.UserId, null, interaction.Id);
                interaction.AddAttachment(attachment);
            }
        }
    }
}
