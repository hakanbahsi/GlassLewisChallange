using FluentAssertions;
using GlassLewisChallange.Application.Companies.Update;
using GlassLewisChallange.Application.Exceptions;
using GlassLewisChallange.Domain.Entities;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using GlassLewisChallange.Tests.Helpers;
using MediatR;
using Moq;

namespace GlassLewisChallange.Tests.Application.Handlers;

public class UpdateCompanyHandlerTests
{
    private readonly Mock<IGlassLewisContext> _contextMock;
    private readonly Mock<IIdProtector> _protectorMock;
    private readonly UpdateCompanyHandler _handler;

    private readonly Company _existingCompany;
    private readonly Guid _companyId = Guid.NewGuid();
    private const string EncryptedId = "encrypted-guid";

    public UpdateCompanyHandlerTests()
    {
        _contextMock = new Mock<IGlassLewisContext>();
        _protectorMock = new Mock<IIdProtector>();

        _existingCompany = new Company("OldName", "OldEx", "OLD", "US0000000000", null);
        typeof(Company).GetProperty("Id")!.SetValue(_existingCompany, _companyId);

        var companies = new List<Company> { _existingCompany }.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(companies.Object);

        _protectorMock.Setup(p => p.Unprotect(EncryptedId)).Returns(_companyId);

        _handler = new UpdateCompanyHandler(_contextMock.Object, _protectorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateCompany_WhenExists()
    {
        // Arrange
        var command = new UpdateCompanyCommand
        {
            Id = EncryptedId,
            Name = "NewName",
            Exchange = "NewEx",
            Ticker = "NEW",
            Isin = "US1234567890",
            Website = "https://updated.com"
        };

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _existingCompany.Name.Should().Be("NewName");
        _existingCompany.Isin.Should().Be("US1234567890");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCompanyNotFound()
    {
        // Arrange
        var fakeId = "fake-id";
        var fakeGuid = Guid.NewGuid();

        _protectorMock.Setup(p => p.Unprotect(fakeId)).Returns(fakeGuid);

        var emptyCompanies = new List<Company>().AsQueryable().BuildMockDbSet();
        _contextMock.Setup(x => x.Companies).Returns(emptyCompanies.Object);

        var command = new UpdateCompanyCommand
        {
            Id = fakeId,
            Name = "Doesn't matter",
            Exchange = "x",
            Ticker = "x",
            Isin = "x"
        };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Company*");
    }
}
