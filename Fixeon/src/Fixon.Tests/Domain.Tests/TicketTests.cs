using Fixeon.Domain.Application.Dtos.Requests;
using Fixeon.Domain.Application.Services;
using Fixon.Tests.MockRepository;

namespace Fixon.Tests.Domain.Tests
{
    [TestClass]
    public class TicketTests
    {
        public TicketServices _services;

        public TicketTests()
        {
            _services = new TicketServices(new FakeTicketRepository());
        }

        [TestMethod]
        public async Task WhenValidCreateTicketRequestReturnTrue()
        {
            var request = new CreateTicketRequest
            {
                Title = "Primeiro chamado",
                Description = "Primeiro chamado realizado para a honra e glória de Deus Pai",
                Category = "Máquinas",
                CreateByUserId = "e7b7f8e4-6d99-4f3e-99aa-f5a7b87b9e71",
                CreateByUsername = "Kaique",
                Priority = Fixeon.Domain.Core.Enums.EPriority.High,
                SecondAttachment = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            };

            var response = await _services.CreateTicket(request);

            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public async Task WhenIValidCreateTicketRequestReturnFalse()
        {
            var request = new CreateTicketRequest
            {
                Title = "",
                Description = "",
                Category = "",
                CreateByUserId = "3e-99aa-f5a7b87b9e71",
                CreateByUsername = "Kaique",
                Priority = Fixeon.Domain.Core.Enums.EPriority.High
            };

            var response = await _services.CreateTicket(request);

            Assert.IsFalse(response.Success);
        }

        [TestMethod]
        public async Task WhenCreateNewValidInteractionAndAssignToTicketReturnTrue()
        {
            var request = new CreateInteractionRequest
            {
                Message = "Mensagem teste passando",
                CreatedByUserId = "e7b7f8e4-6d99-4f3e-99aa-f5a7b87b9e71",
                CreatedByUserName = "Kaique",
                TicketId = Guid.Parse("e7b7f8e4-6d99-4f3e-99aa-f5a7b87b9e71")
            };

            var response = await _services.CreateTicketInteraction(request);

            Assert.IsTrue(response.Success);
        }
    }
}
