using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Services;
using Fixeon.Domain.Core.Enums;
using Fixon.Tests.MockRepository;

namespace Fixon.Tests.Domain.Tests
{
    [TestClass]
    public class TicketTests
    {
        public TicketServices _services;

        public TicketTests()
        {
            _services = new TicketServices(new FakeTicketRepository(), new FakeUnitOfWork(), new FakeStorageService(), new FakeTenantContext(), new FakeOrganizationServices(), new FakeNotificationServices());
        }

        [TestMethod]
        public async Task VerifyTicketAnalysisMethod()
        {
            var analysis = await _services.GetDashboardTickets();

            Assert.IsTrue(analysis.Success && analysis.Data.TicketAnalysisResponse.Total > 0 && analysis.Data.analystTicketsAnalyses.Count > 0);
        }

        [TestMethod]
        public async Task VerifyCreateTicketService()
        {
            var request = new CreateTicketRequest()
            {
                Title = "Erro ao acessar painel administrativo",
                Description = "Usuário não consegue acessar o painel administrativo. Apresenta erro 500.",
                Category = "Acesso",
                Departament = "TI",
                Priority = EPriority.High
            };

            var result = await _services.CreateTicket(request);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task VerifyCreateTicketServiceValidation()
        {
            var request = new CreateTicketRequest()
            {
                Title = "",
                Description = "",
                Category = "Acesso",
                Departament = "TI",
                Priority = EPriority.High
            };

            var result = await _services.CreateTicket(request);

            Assert.IsTrue(result.Errors.Any() && !result.Success);
        }

        [TestMethod]
        public async Task VerifyCreateInteraction()
        {
            var ticket = TicketsMock.Tickets.First();

            var request = new CreateInteractionRequest
            {
                TicketId = ticket.Id,
                Message = "Ticket encerrado com sucesso."
            };

            var result = await _services.CreateTicketInteraction(request);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task VerifyAssignTicketToAnalyst()
        {
            var ticket = TicketsMock.Tickets[8];
            var analyst = TicketsMock.Analysts[4];

            var request = new CreateAssignTicketRequest
            {
                TicketId = ticket.Id,
                AnalystId = analyst.AnalystId,
                AnalystEmail = analyst.AnalystEmail
            };

            var result = await _services.AssignTicketTo(request);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]

        public async Task VerifyChangeTicketCategory()
        {
            var ticket = TicketsMock.Tickets[8];

            var request = new ChangeTicketCategory
            {
                Id = ticket.Id,
                Category = "Nova categoria de ticket"
            };

            var result = await _services.ChangeTicketCategory(request);

            Assert.IsTrue(result.Success && result.Data.Category.Equals(request.Category));
        }
    }
}
