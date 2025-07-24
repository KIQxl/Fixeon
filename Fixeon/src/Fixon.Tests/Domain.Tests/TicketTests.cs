using Fixeon.Domain.Application.Dtos;
using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Services;
using Fixeon.Domain.Core.Entities;
using Fixeon.Domain.Core.Enums;
using Fixeon.Domain.Core.ValueObjects;
using Fixon.Tests.MockRepository;

namespace Fixon.Tests.Domain.Tests
{
    [TestClass]
    public class TicketTests
    {
        public TicketServices _services;

        public TicketTests()
        {
            _services = new TicketServices(new FakeTicketRepository(), new FakeFileService(), new FakeUnitOfWork());
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
                CreateByUserId = Guid.NewGuid().ToString(),
                CreateByUsername = "João Souza",
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
                CreateByUserId = "0000000000000000000000000000000000000000000000000000000",
                CreateByUsername = "João Souza",
                Priority = EPriority.High
            };

            var result = await _services.CreateTicket(request);

            Assert.IsTrue(result.Errors.Any() && !result.Success);
        }
    }
}
