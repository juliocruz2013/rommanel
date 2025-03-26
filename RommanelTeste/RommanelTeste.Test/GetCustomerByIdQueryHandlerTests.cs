using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RommanelTeste.Application.Customer.Dtos;
using RommanelTeste.Application.Customer.Queries.GetCustomerById;
using RommanelTeste.Common;
using RommanelTeste.Domain.Entities;
using RommanelTeste.Domain.Enumerators;

namespace RommanelTeste.Test;

public class GetCustomerByIdQueryHandlerTests : IClassFixture<RommanelTesteContextFixture>
{
    private readonly RommanelTesteContextFixture _fixture;
    private readonly Mock<ILogger<GetCustomerByIdQueryHandler>> _loggerMock;
    private readonly GetCustomerByIdQueryHandler _handler;

    public GetCustomerByIdQueryHandlerTests(RommanelTesteContextFixture fixture)
    {
        _fixture = fixture;
        _loggerMock = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
        _handler = new GetCustomerByIdQueryHandler(_loggerMock.Object, fixture.Context);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenUserIdIsInvalid()
    {
        // Arrange
        var request = new GetCustomerByIdQueryRequest
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
        var request = new GetCustomerByIdQueryRequest
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
    public async Task Handle_ShouldReturnCustomer_WhenDataIsValid()
    {
        // Arrange
        var customer = new Customer();
        customer.SetCustomer("user456","Ana Cliente","99988877766","ana@email.com",DateTime.Now.AddYears(-30),"11999999999",false,null,false,
         Address.SetAddress(customer.Id,"01311923","Rua Nova","1234","Atualizado","São Paulo","SP"));

        await _fixture.Context.Customer.AddAsync(customer);
        await _fixture.Context.SaveChangesAsync(new CancellationToken());

        var request = new GetCustomerByIdQueryRequest
        {
            Id = customer.Id,
            UserId = "user456"
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.HttpStatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Data.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(customer.Id);
        result.Data.Name.Should().Be("Ana Cliente");
    }
}
