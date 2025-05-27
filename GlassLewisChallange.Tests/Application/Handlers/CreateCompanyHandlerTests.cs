using AutoMapper;
using FluentAssertions;
using GlassLewisChallange.Application.Companies.Create;
using GlassLewisChallange.Domain.Entities;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using GlassLewisChallange.Tests.Helpers;
using Moq;

namespace GlassLewisChallange.Tests.Application.Handlers;

public class CreateCompanyHandlerTests
{
    private readonly Mock<IGlassLewisContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IIdProtector> _protectorMock;
    private readonly CreateCompanyHandler _handler;
    private readonly List<Company> _fakeCompanies;

    public CreateCompanyHandlerTests()
    {
        _contextMock = new Mock<IGlassLewisContext>();
        _mapperMock = new Mock<IMapper>();
        _protectorMock = new Mock<IIdProtector>();

        _fakeCompanies = new List<Company>
        {
            new Company("Apple", "NASDAQ", "AAPL", "US0378331005", "https://apple.com")
        };

        var dbSetMock = _fakeCompanies.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(dbSetMock.Object);

        _handler = new CreateCompanyHandler(_contextMock.Object, _mapperMock.Object, _protectorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenIsinAlreadyExists()
    {
        // Arrange
        var existingCompany = new Company("Apple", "NASDAQ", "AAPL", "US0378331005", "https://apple.com");

        var companies = new List<Company> { existingCompany };
        var mockSet = companies.AsQueryable().BuildMockDbSet();

        _contextMock.Setup(x => x.Companies).Returns(mockSet.Object);

        _mapperMock.Setup(m => m.Map<Company>(It.IsAny<CreateCompanyCommand>()))
            .Returns((CreateCompanyCommand cmd) =>
                new Company(cmd.Name, cmd.Exchange, cmd.Ticker, cmd.Isin, cmd.Website));

        var command = new CreateCompanyCommand
        {
            Name = "Apple",
            Exchange = "NASDAQ",
            Ticker = "AAPL",
            Isin = "US0378331005",
            Website = "https://apple.com"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }


    [Fact]
    public async Task Handle_ShouldReturnEncryptedId_WhenCompanyIsCreated()
    {
        // Arrange
        var command = new CreateCompanyCommand
        {
            Name = "British Airways Plc",
            Exchange = "Pink Sheets",
            Ticker = "BAIRY",
            Isin = "US1104193065",
            Website = "https://tesla.com"
        };

        _contextMock.Setup(x => x.Companies).Returns(new List<Company>().AsQueryable().BuildMockDbSet().Object);

        var createdEntity = new Company(command.Name, command.Exchange, command.Ticker, command.Isin, command.Website);
        _mapperMock.Setup(m => m.Map<Company>(command)).Returns(createdEntity);
        _protectorMock.Setup(p => p.Protect(It.IsAny<Guid>())).Returns("encrypted-guid");

        var dbSetMock = new List<Company>().AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(dbSetMock.Object);

        _contextMock.Setup(x => x.Companies.Add(It.IsAny<Company>()));
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be("encrypted-guid");
    }
}
