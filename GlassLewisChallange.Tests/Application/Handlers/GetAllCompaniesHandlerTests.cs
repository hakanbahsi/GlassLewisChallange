using AutoMapper;
using FluentAssertions;
using GlassLewisChallange.Application.Companies.GetAll;
using GlassLewisChallange.Domain.Entities;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using GlassLewisChallange.Tests.Helpers;
using Moq;

namespace GlassLewisChallange.Tests.Application.Handlers;

public class GetAllCompaniesHandlerTests
{
    private readonly Mock<IGlassLewisContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IIdProtector> _protectorMock;
    private readonly GetAllCompaniesHandler _handler;

    private readonly Guid _companyId1 = Guid.NewGuid();
    private readonly Guid _companyId2 = Guid.NewGuid();

    public GetAllCompaniesHandlerTests()
    {
        _contextMock = new Mock<IGlassLewisContext>();
        _mapperMock = new Mock<IMapper>();
        _protectorMock = new Mock<IIdProtector>();

        var company1 = new Company("Apple", "NASDAQ", "AAPL", "US0378331005", "http://apple.com");
        typeof(Company).GetProperty("Id")!.SetValue(company1, _companyId1);

        var company2 = new Company("Heineken", "Euronext", "HEIA", "NL0000009165", null);
        typeof(Company).GetProperty("Id")!.SetValue(company2, _companyId2);

        var companyList = new List<Company> { company1, company2 };
        var mockSet = companyList.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(mockSet.Object);

        _mapperMock.Setup(m => m.Map<List<GetAllCompaniesDto>>(companyList)).Returns(new List<GetAllCompaniesDto>
        {
            new GetAllCompaniesDto { Id = "", Name = "Apple", Exchange = "NASDAQ", Ticker = "AAPL", Isin = "US0378331005", Website = "http://apple.com" },
            new GetAllCompaniesDto { Id = "", Name = "Heineken", Exchange = "Euronext", Ticker = "HEIA", Isin = "NL0000009165", Website = null }
        });

        _protectorMock.Setup(p => p.Protect(_companyId1)).Returns("encrypted-1");
        _protectorMock.Setup(p => p.Protect(_companyId2)).Returns("encrypted-2");

        _handler = new GetAllCompaniesHandler(_contextMock.Object, _mapperMock.Object, _protectorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfCompanies_WhenCompaniesExist()
    {
        // Arrange
        var query = new GetAllCompaniesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Id.Should().Be("encrypted-1");
        result[1].Id.Should().Be("encrypted-2");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCompaniesExist()
    {
        // Arrange
        var emptySet = new List<Company>().AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(emptySet.Object);
        _mapperMock.Setup(m => m.Map<List<GetAllCompaniesDto>>(It.IsAny<List<Company>>())).Returns(new List<GetAllCompaniesDto>());

        var query = new GetAllCompaniesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
