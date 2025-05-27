using FluentAssertions;
using GlassLewisChallange.Application.Companies.Update;
using GlassLewisChallange.Infrastructure.Security;
using Moq;

namespace GlassLewisChallange.Tests.Application.Validators;

public class UpdateCompanyValidatorTests
{
    private readonly UpdateCompanyValidator _validator;
    private readonly Mock<IIdProtector> _protectorMock;

    public UpdateCompanyValidatorTests()
    {
        _protectorMock = new Mock<IIdProtector>();

        _protectorMock.Setup(p => p.CanUnprotect("valid-id")).Returns(true);
        _protectorMock.Setup(p => p.CanUnprotect("invalid-id")).Returns(false);

        _validator = new UpdateCompanyValidator(_protectorMock.Object);
    }

    [Fact]
    public void Should_Fail_When_Id_Is_Invalid()
    {
        var model = new UpdateCompanyCommand
        {
            Id = "invalid-id",
            Name = "Apple",
            Exchange = "NASDAQ",
            Ticker = "AAPL",
            Isin = "US1234567890"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_Pass_When_Id_Is_Valid()
    {
        var model = new UpdateCompanyCommand
        {
            Id = "valid-id",
            Name = "Apple",
            Exchange = "NASDAQ",
            Ticker = "AAPL",
            Isin = "US1234567890"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Website_Is_Invalid()
    {
        var model = new UpdateCompanyCommand
        {
            Name = "Updated",
            Exchange = "X",
            Ticker = "X",
            Isin = "US1234567890",
            Website = "invalid-url"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Website");
    }

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var model = new UpdateCompanyCommand
        {
            Id = "valid-id",
            Name = "Updated",
            Exchange = "X",
            Ticker = "X",
            Isin = "US1234567890",
            Website = "https://updated.com"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }

}
