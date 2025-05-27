using AutoMapper;
using FluentAssertions;
using GlassLewisChallange.Application.Companies.GetById;
using GlassLewisChallange.Application.Exceptions;
using GlassLewisChallange.Domain.Entities;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using GlassLewisChallange.Tests.Helpers;
using Moq;

namespace GlassLewisChallange.Tests.Application.Handlers;

public class GetCompanyByIdHandlerTests
{
    private readonly Mock<IGlassLewisContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IIdProtector> _protectorMock;
    private readonly GetCompanyByIdHandler _handler;

    private readonly Company _testCompany;
    private readonly Guid _testGuid = Guid.NewGuid();
    private const string EncryptedId = "encrypted-guid";

    public GetCompanyByIdHandlerTests()
    {
        _contextMock = new Mock<IGlassLewisContext>();
        _mapperMock = new Mock<IMapper>();
        _protectorMock = new Mock<IIdProtector>();

        _testCompany = new Company("Apple", "NASDAQ", "AAPL", "US0378331005", "https://apple.com");
        typeof(Company).GetProperty("Id")!.SetValue(_testCompany, _testGuid);

        var companies = new List<Company> { _testCompany }.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(companies.Object);

        _mapperMock.Setup(m => m.Map<GetCompanyByIdDto>(_testCompany))
            .Returns(new GetCompanyByIdDto
            {
                Id = EncryptedId,
                Name = _testCompany.Name,
                Exchange = _testCompany.Exchange,
                Ticker = _testCompany.Ticker,
                Isin = _testCompany.Isin,
                Website = _testCompany.Website
            });

        _protectorMock.Setup(p => p.Unprotect(EncryptedId)).Returns(_testGuid);
        _protectorMock.Setup(p => p.Protect(_testGuid)).Returns(EncryptedId);

        _handler = new GetCompanyByIdHandler(_contextMock.Object, _mapperMock.Object, _protectorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCompanyDto_WhenCompanyExists()
    {
        // Arrange
        var query = new GetCompanyByIdQuery { Id = EncryptedId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(EncryptedId);
        result.Name.Should().Be("Apple");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var nonExistingGuid = Guid.NewGuid();
        var nonExistingId = "fake-id";

        _protectorMock.Setup(p => p.Unprotect(nonExistingId)).Returns(nonExistingGuid);
        var emptyCompanies = new List<Company>().AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(emptyCompanies.Object);

        var query = new GetCompanyByIdQuery { Id = nonExistingId };

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Company*");
    }
}
