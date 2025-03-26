using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RommanelTeste.Application.Customer.Queries.GetCustomers;
using RommanelTeste.Common;
using RommanelTeste.Domain.Entities;
using RommanelTeste.Domain.Enumerators;

namespace RommanelTeste.Test;

public class GetCustomersQueryHandlerTests : IClassFixture<RommanelTesteContextFixture>
{
    private readonly RommanelTesteContextFixture _fixture;
    private readonly Mock<ILogger<GetCustomersQueryHandler>> _loggerMock;
    private readonly GetCustomersQueryHandler _handler;

    public GetCustomersQueryHandlerTests(RommanelTesteContextFixture fixture)
    {
        _fixture = fixture;
        _loggerMock = new Mock<ILogger<GetCustomersQueryHandler>>();
        _handler = new GetCustomersQueryHandler(_loggerMock.Object, _fixture.Context);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenUserIdIsInvalid()
    {
        // Arrange
        var request = new GetCustomersQueryRequest
        {
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
    public async Task Handle_ShouldReturnEmptyList_WhenNoCustomersExist()
    {
        // Arrange
        var request = new GetCustomersQueryRequest
        {
            UserId = "user123"
        };

        var existingCustomers = await _fixture.Context.Customer.ToListAsync();
        _fixture.Context.Customer.RemoveRange(existingCustomers);
        await _fixture.Context.SaveChangesAsync(new CancellationToken());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfCustomers_WhenTheyExist()
    {
        // Arrange
        var customer1 = new Customer();
        customer1.SetCustomer("user1", "João", "12345678900", "joao@email.com", DateTime.Now.AddYears(-30), "11911112222", false, null, false,
        Address.SetAddress(customer1.Id, "01311923", "Rua Nova", "1234", "Atualizado", "São Paulo", "SP"));


        var customer2 = new Customer();
        customer2.SetCustomer("user2", "Maria", "98765432100", "maria@email.com", DateTime.Now.AddYears(-25), "11933334444", false, null, false,
        Address.SetAddress(customer2.Id, "01311924", "Rua Nova 2", "12345", "Atualizado 2", "São Paulo", "SP"));

        await _fixture.Context.Customer.AddRangeAsync(customer1, customer2);
        await _fixture.Context.SaveChangesAsync(new CancellationToken());

        var request = new GetCustomersQueryRequest
        {
            UserId = "userAdmin"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCountGreaterThanOrEqualTo(2);
        result.Data.Select(x => x.Name).Should().Contain(["João", "Maria"]);
    }
}
