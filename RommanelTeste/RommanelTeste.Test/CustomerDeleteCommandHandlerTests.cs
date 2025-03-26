using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RommanelTeste.Application.Customer.Commands.CustomerDelete;
using RommanelTeste.Common;
using RommanelTeste.Domain.Entities;
using RommanelTeste.Domain.Enumerators;

namespace RommanelTeste.Test;

public class CustomerDeleteCommandHandlerTests : IClassFixture<RommanelTesteContextFixture>
{
    private readonly Mock<ILogger<CustomerDeleteCommandHandler>> _loggerMock;
    private readonly RommanelTesteContextFixture _fixture;
    private readonly CustomerDeleteCommandHandler _handler;

    public CustomerDeleteCommandHandlerTests(RommanelTesteContextFixture fixture)
    {
        _fixture = fixture;
        _loggerMock = new Mock<ILogger<CustomerDeleteCommandHandler>>();
        _handler = new CustomerDeleteCommandHandler(_loggerMock.Object, fixture.Context);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenUserIdIsInvalid()
    {
        // Arrange
        var request = new CustomerDeleteCommandRequest
        {
            Id = 1,
            UserId = ""
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
        var request = new CustomerDeleteCommandRequest
        {
            Id = 9999, 
            UserId = "user123"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be(29);
        result.Error.Message.Should().BeEquivalentTo(TypeError.CustomerNotFound.GetDescription());
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldDeleteCustomer_WhenDataIsValid()
    {
        // Arrange
        var customer = new Customer();
        customer.SetCustomer("user789", "João Delete", "12345678900", "joaodelete@email.com", DateTime.Now.AddYears(-40), "11999999999", false, null, false, null);

        await _fixture.Context.Customer.AddAsync(customer);
        await _fixture.Context.SaveChangesAsync(new CancellationToken());

        var request = new CustomerDeleteCommandRequest
        {
            Id = customer.Id,
            UserId = "user789"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
        result.Data.Message.Should().Be("Cliente excluído com sucesso!");

        var deleted = await _fixture.Context.Customer.FirstOrDefaultAsync(x => x.Id == customer.Id);
        deleted.Should().BeNull();
    }
}
