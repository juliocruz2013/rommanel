using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RommanelTeste.Application.Customer.Commands.CustomerCreate;
using RommanelTeste.Application.Customer.Dtos;
using RommanelTeste.Common;
using RommanelTeste.Domain.Entities;
using RommanelTeste.Domain.Enumerators;

namespace RommanelTeste.Test;

public class CustomerCreateCommandHandlerTests : IClassFixture<RommanelTesteContextFixture>
{
    private readonly Mock<ILogger<CustomerCreateCommandHandler>> _loggerMock;
    private readonly RommanelTesteContextFixture _fixture;
    private readonly CustomerCreateCommandHandler _handler;

    public CustomerCreateCommandHandlerTests(RommanelTesteContextFixture fixture)
    {
        _fixture = fixture;
        _loggerMock = new Mock<ILogger<CustomerCreateCommandHandler>>();
        _handler = new CustomerCreateCommandHandler(_loggerMock.Object, fixture.Context);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenUserIdIsInvalid()
    {
        // Arrange
        var request = new CustomerCreateCommandRequest
        {
            UserId = "",
            Name = "João Silva",
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
        result.Error.Should().NotBeNull();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Error.Code.Should().Be(27);
        result.Error.Message.Should().BeEquivalentTo(TypeError.InvalidId.GetDescription());
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenCustomerAlreadyExists()
    {
        // Arrange
        var existingCustomer = new Customer();
        existingCustomer.SetCustomer("user123", "Maria", "12345678900", "maria@email.com", DateTime.Now.AddYears(-25), "11999999999", false, null, false, null);

        await _fixture.Context.Customer.AddAsync(existingCustomer);
        await _fixture.Context.SaveChangesAsync(new CancellationToken());

        var request = new CustomerCreateCommandRequest
        {
            UserId = "user456",
            Name = "Maria",
            Email = "maria@email.com",
            DocumentNumber = "12345678900",
            Phone = "11900000000",
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
        result.Error.Should().NotBeNull();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Error.Code.Should().Be(31);
        result.Error.Message.Should().BeEquivalentTo(TypeError.CustomerRegisteredWithTheSameData.GetDescription());
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldCreateCustomer_WhenDataIsValid()
    {
        // Arrange
        var request = new CustomerCreateCommandRequest
        {
            UserId = Guid.NewGuid().ToString(),
            Name = "Carlos Andrade",
            Email = "carlos@email.com",
            DocumentNumber = "98765432100",
            Phone = "11988888888",
            BirthDate = DateTime.Now.AddYears(-40),
            IsCompany = false,
            StateRegistration = null,
            IsExempt = false,
            Address = new AddressDto
            {
                ZipCode = "01311923",
                Street = "Av. Paulista",
                Number = "1000",
                Neighborhood = "Bela Vista",
                City = "São Paulo",
                State = "SP"
            }
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.Created);
        result.Data.Should().NotBeNull();
        result.Data.Message.Should().Be("Cliente cadastrado com sucesso!");

        var createdCustomer = await _fixture.Context.Customer.FirstOrDefaultAsync(x => x.Email == request.Email && x.DocumentNumber == request.DocumentNumber);

        createdCustomer.Should().NotBeNull();
        createdCustomer?.Name.Should().Be("Carlos Andrade");
        createdCustomer?.DocumentNumber.Should().Be("98765432100");
        createdCustomer?.Email.Should().Be("carlos@email.com");
        createdCustomer?.Phone.Should().Be("11988888888");
        createdCustomer?.IsCompany.Should().BeFalse();
        createdCustomer?.IsExempt.Should().BeFalse();
    }
}
