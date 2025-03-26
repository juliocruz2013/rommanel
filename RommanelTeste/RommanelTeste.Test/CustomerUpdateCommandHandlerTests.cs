using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RommanelTeste.Application.Customer.Commands.CustomerUpdate;
using RommanelTeste.Application.Customer.Dtos;
using RommanelTeste.Common;
using RommanelTeste.Domain.Entities;
using RommanelTeste.Domain.Enumerators;

namespace RommanelTeste.Test
{
    public class CustomerUpdateCommandHandlerTests : IClassFixture<RommanelTesteContextFixture>
    {
        private readonly Mock<ILogger<CustomerUpdateCommandHandler>> _loggerMock;
        private readonly RommanelTesteContextFixture _fixture;
        private readonly CustomerUpdateCommandHandler _handler;

        public CustomerUpdateCommandHandlerTests(RommanelTesteContextFixture fixture)
        {
            _fixture = fixture;
            _loggerMock = new Mock<ILogger<CustomerUpdateCommandHandler>>();
            _handler = new CustomerUpdateCommandHandler(_loggerMock.Object, fixture.Context);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenUserIdIsInvalid()
        {
            // Arrange
            var request = new CustomerUpdateCommandRequest
            {
                Id = 1,
                UserId = "",
                Name = "João Atualizado",
                Email = "joao@email.com",
                DocumentNumber = "12345678900",
                Phone = "11999999999",
                BirthDate = DateTime.Now.AddYears(-30),
                Address = new AddressDto
                {
                    ZipCode = "01234567",
                    Street = "Rua Teste",
                    Number = "123",
                    Neighborhood = "Centro",
                    City = "São Paulo",
                    State = "SP"
                }
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.HttpStatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be(27);
            result.Error.Message.Should().BeEquivalentTo(TypeError.InvalidId.GetDescription());
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenCustomerNotFound()
        {
            // Arrange
            var request = new CustomerUpdateCommandRequest
            {
                Id = 9999,
                UserId = "user123",
                Name = "Cliente Inexistente",
                Email = "naoexiste@email.com",
                DocumentNumber = "00000000000",
                Phone = "11900000000",
                BirthDate = DateTime.Now.AddYears(-30),
                Address = new AddressDto
                {
                    ZipCode = "01234567",
                    Street = "Rua Fantasma",
                    Number = "0",
                    Neighborhood = "Desconhecido",
                    City = "Cidade",
                    State = "ZZ"
                }
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.HttpStatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error.Code.Should().Be(29);
            result.Error.Message.Should().BeEquivalentTo(TypeError.CustomerNotFound.GetDescription());
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenAnotherCustomerHasSameEmailOrDocument()
        {
            // Arrange
            var customer1 = new Customer();
            customer1.SetCustomer("user1", "Fulano", "11122233344", "fulano@email.com", DateTime.Now.AddYears(-25), "11999990000", false, null, false, null);

            var customer2 = new Customer();
            customer2.SetCustomer("user2", "Ciclano", "55566677788", "ciclano@email.com", DateTime.Now.AddYears(-30), "11988887777", false, null, false, null);

            await _fixture.Context.Customer.AddRangeAsync(customer1, customer2);
            await _fixture.Context.SaveChangesAsync(new CancellationToken());

            var request = new CustomerUpdateCommandRequest
            {
                Id = customer2.Id,
                UserId = "user2",
                Name = "Ciclano Atualizado",
                Email = "fulano@email.com",
                DocumentNumber = "11122233344",
                Phone = "11900001111",
                BirthDate = DateTime.Now.AddYears(-35),
                Address = new AddressDto
                {
                    ZipCode = "01234567",
                    Street = "Rua Atualizada",
                    Number = "456",
                    Neighborhood = "Novo Bairro",
                    City = "São Paulo",
                    State = "SP"
                }
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.HttpStatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error.Code.Should().Be(31);
            result.Error.Message.Should().BeEquivalentTo(TypeError.CustomerRegisteredWithTheSameData.GetDescription());
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldUpdateCustomer_WhenDataIsValid()
        {
            // Arrange
            var customer = new Customer();
            customer.SetCustomer("user789", "Antônio", "99988877766", "antonio@email.com", DateTime.Now.AddYears(-40), "11977776666", false, null, false, null);

            await _fixture.Context.Customer.AddAsync(customer);
            await _fixture.Context.SaveChangesAsync(new CancellationToken());

            var request = new CustomerUpdateCommandRequest
            {
                Id = customer.Id,
                UserId = "user789",
                Name = "Antônio Atualizado",
                Email = "antonio_atualizado@email.com",
                DocumentNumber = "99988877766",
                Phone = "11966665555",
                BirthDate = DateTime.Now.AddYears(-40),
                IsCompany = false,
                IsExempt = false,
                StateRegistration = null,
                Address = new AddressDto
                {
                    ZipCode = "01311923",
                    Street = "Rua Nova",
                    Number = "1234",
                    Neighborhood = "Atualizado",
                    City = "São Paulo",
                    State = "SP"
                }
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.HttpStatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be("Antônio Atualizado");

            var updated = await _fixture.Context.Customer.FirstOrDefaultAsync(x => x.Id == customer.Id);
            updated.Should().NotBeNull();
            updated?.Name.Should().Be("Antônio Atualizado");
            updated?.Email.Should().Be("antonio_atualizado@email.com");
            updated?.Phone.Should().Be("11966665555");
        }
    }
}
