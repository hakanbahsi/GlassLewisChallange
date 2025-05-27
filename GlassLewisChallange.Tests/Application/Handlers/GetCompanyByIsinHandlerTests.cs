using AutoMapper;
using FluentAssertions;
using GlassLewisChallange.Application.Companies.GetCompanyByIsin;
using GlassLewisChallange.Application.Exceptions;
using GlassLewisChallange.Domain.Entities;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using GlassLewisChallange.Tests.Helpers;
using Moq;

namespace GlassLewisChallange.Tests.Application.Handlers;

public class GetCompanyByIsinHandlerTests
{
    private readonly Mock<IGlassLewisContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IIdProtector> _protectorMock;
    private readonly GetCompanyByIsinHandler _handler;

    private readonly Company _company;
    private const string TestIsin = "US0378331005";
    private const string EncryptedId = "encrypted-id";

    public GetCompanyByIsinHandlerTests()
    {
        _contextMock = new Mock<IGlassLewisContext>();
        _mapperMock = new Mock<IMapper>();
        _protectorMock = new Mock<IIdProtector>();

        _company = new Company("Apple", "NASDAQ", "AAPL", TestIsin, "https://apple.com");

        var companyList = new List<Company> { _company };
        var mockSet = companyList.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(mockSet.Object);

        _mapperMock.Setup(m => m.Map<GetCompanyByIsinDto>(_company)).Returns(new GetCompanyByIsinDto
        {
            Id = EncryptedId,
            Name = _company.Name,
            Exchange = _company.Exchange,
            Ticker = _company.Ticker,
            Isin = _company.Isin,
            Website = _company.Website
        });

        _protectorMock.Setup(p => p.Protect(It.IsAny<Guid>())).Returns(EncryptedId);

        _handler = new GetCompanyByIsinHandler(_contextMock.Object, _mapperMock.Object, _protectorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCompanyDto_WhenIsinExists()
    {
        // Arrange
        var query = new GetCompanyByIsinQuery { Isin = TestIsin };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Apple");
        result.Isin.Should().Be(TestIsin);
        result.Id.Should().Be(EncryptedId);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenIsinDoesNotExist()
    {
        // Arrange
        var query = new GetCompanyByIsinQuery { Isin = "XX0000000000" };

        var emptySet = new List<Company>().AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(emptySet.Object);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Company*");
    }
}
