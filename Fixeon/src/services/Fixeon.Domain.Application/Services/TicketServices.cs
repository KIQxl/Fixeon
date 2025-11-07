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

namespace Fixeon.Domain.Application.Services
{
    public class TicketServices : ITicketServices
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageServices _storageServices;
        private readonly ITenantContextServices _tenantContext;
        private readonly IOrganizationResolver _organizationResolver;
        private readonly ITicketNotificationServices _notificationServices;
        private readonly ICompanyResolver _companyresolver;

        public TicketServices(
            ITicketRepository ticketRepository,
            IUnitOfWork unitOfWork,
            IStorageServices storageServices,
            ITenantContextServices tenantContext,
            IOrganizationResolver organizationServices,
            ITicketNotificationServices notificationServices,
            ICompanyResolver companyResolver)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _storageServices = storageServices;
            _tenantContext = tenantContext;
            _organizationResolver = organizationServices;
            _notificationServices = notificationServices;
            _companyresolver = companyResolver;
        }

        public async Task<Response<TicketResponse>> CreateTicket(CreateTicketRequest request)
        {
            var validationResult = request.Validate();

            if (!validationResult.IsValid)
                return new Response<TicketResponse>(validationResult.Errors.Select(x => x.ErrorMessage).ToList(), EErrorType.BadRequest);

            var currentUser = await _tenantContext.GetCurrentUser();

            var organization = await _organizationResolver.GetOrganization(_tenantContext.OrganizationId.Value);

            var customer = new User { UserId = currentUser.UserId.ToString(), UserEmail = currentUser.UserEmail, OrganizationId = organization.OrganizationId, OrganizationName = organization.OrganizationName };

            var ticket = TicketMapper.ToEntity(request, customer);

            await SetTicketSLA(ticket);

            await ProccessTicketAttachment(ticket, request.Attachments);

            try
            {
                await _ticketRepository.CreateTicket(ticket);

                var result = await _unitOfWork.Commit();

                await _notificationServices.NotifyAnalystTeam(ticket, _tenantContext.TenantId);

                await _notificationServices.NotifyRequesterAsync(_tenantContext.UserEmail);

                if (!result)
                    return new Response<TicketResponse>("Não foi possível realizar a abertura do chamado.", EErrorType.ServerError);

                return new Response<TicketResponse>(ticket.ToResponse());
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<TicketResponse>($"{message}", EErrorType.ServerError);
            }
        }

        public async Task<Response<TicketResponse>> ChangeTicketCategoryAndDepartament(ChangeTicketCategoryAndDepartament request)
        {
            try
            {
                var ticket = await _ticketRepository.GetTicketByIdAsync(request.Id);
                if (ticket is null)
                    return new Response<TicketResponse>("Ticket não encontrado", EErrorType.NotFound);

                var organization = await _organizationResolver.GetOrganization(request.OrganizationId);

                if (organization is null)
                    return new Response<TicketResponse>("Organização não encontrada", EErrorType.NotFound);

                if (organization.Categories.Any(x => x.Id == request.CategoryId) && organization.Departaments.Any(x => x.Id == request.DepartamentId))
                {
                    ticket.ChangeCategory(organization.Categories.FirstOrDefault(x => x.Id == request.CategoryId)!.Name);
                    ticket.ChanceDepartament(organization.Departaments.FirstOrDefault(x => x.Id == request.DepartamentId)!.Name);

                    await _ticketRepository.UpdateTicket(ticket);
                    await _unitOfWork.Commit();
                }
                else
                    return new Response<TicketResponse>("Categoria ou departamento não cadastrados para essa organização.", EErrorType.NotFound);

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

        public async Task<Response<List<TicketResponse>>> GetAllTicketsAsync()
        {
            try
            {
                var tickets = await _ticketRepository.GetAllTicketsAsync();

                if (tickets is null)
                    return new Response<List<TicketResponse>>("Tickets não encontrados.", EErrorType.NotFound);

                var responses = tickets.Select(x =>
                {
                    var urls = GetAttachmentsUrl(x.Attachments);
                    return x.ToResponse(urls.Result);
                }).ToList();

                return new Response<List<TicketResponse>>(responses);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<List<TicketResponse>>($"{message}", EErrorType.ServerError);
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

                if (ticket.Status.Equals(request.Status.ToString()))
                    return new Response<TicketResponse>("Essa ação já foi realizada.", EErrorType.BadRequest);

                if (ticket is null)
                    return new Response<TicketResponse>("Ticket não encontrado.", EErrorType.NotFound);

                switch (request.Status)
                {
                    case ETicketStatus.Resolved:
                        if (!ticket.ResolveTicket(new Analyst { AnalystId = _tenantContext.UserId.ToString(), AnalystEmail = _tenantContext.UserEmail }))
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

                var ticketsByDay = await _ticketRepository.GetTicketsByDayAsync();

                var ticketsByHour = await _ticketRepository.GetTicketsByHourAsync();

                var ticketSLAAnalysis = await _ticketRepository.GetTicketsSLAAnalysisByOrganizationAsync();

                var result = new TicketDashboardResponse
                {
                    TicketAnalysisResponse = ticketAnalysis,
                    analystTicketsAnalyses = analystTicketAnalysis,
                    TopAnalystResponse = topAnalystAnalysis,
                    TicketsByDay = ticketsByDay,
                    TicketsByHour = ticketsByHour,
                    TicketSLAAnalysisResponse = ticketSLAAnalysis.ToList()
                };

                return new Response<TicketDashboardResponse>(result);
            }
            catch (Exception ex)
            {
                return new Response<TicketDashboardResponse>(ex.InnerException?.Message ?? ex.Message, EErrorType.ServerError);
            }
        }

        public async Task<Response<bool>> AddTagInTicket(AddTagInTicketRequest request)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(request.TicketId);

            if (ticket is null)
                return new Response<bool>("Ticket não encontrado", EErrorType.NotFound);

            var company = await _companyresolver.GetCompany(_tenantContext.TenantId);

            var tag = company.Tags.FirstOrDefault(t => t.Id == request.TagId);

            if (tag is null)
                return new Response<bool>("Tag não encontrada", EErrorType.NotFound);

            ticket.AddTag(tag.Id);

            try
            {
                await _ticketRepository.UpdateTicket(ticket);
                await _unitOfWork.Commit();

                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(new List<string> { "Tag não encontrada", ex.Message }, EErrorType.BadRequest);
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

        private async Task SetTicketSLA(Ticket ticket)
        {
            if (_tenantContext.OrganizationId.HasValue)
            {
                var SLAs = await _organizationResolver.GetSLAByOrganization(_tenantContext.OrganizationId.Value);

                var firstInteractionSLA = SLAs.FirstOrDefault(x => x.Type == (int)ESLAType.FirstInteraction && x.SLAPriority == ticket.Priority);
                if (firstInteractionSLA != null)
                    ticket.SetFirstInteractionDeadline(firstInteractionSLA.SLAInMinutes);

                var resolutionSLA = SLAs.FirstOrDefault(x => x.Type == (int)ESLAType.Resolution && x.SLAPriority == ticket.Priority);
                if (resolutionSLA != null)
                    ticket.SetResolutionDeadline(resolutionSLA.SLAInMinutes);
            }
        }
    }
}
