using FluentAssertions;
using GlassLewisChallange.Application.Companies.Create;
using GlassLewisChallange.Infrastructure.Security;
using Moq;

namespace GlassLewisChallange.Tests.Application.Validators;

public class CreateCompanyValidatorTests
{
    private readonly CreateCompanyValidator _validator;

    public CreateCompanyValidatorTests()
    {
        var protectorMock = new Mock<IIdProtector>();
        _validator = new CreateCompanyValidator();
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var model = new CreateCompanyCommand
        {
            Name = "",
            Exchange = "NASDAQ",
            Ticker = "AAPL",
            Isin = "US1234567890"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Fail_When_Isin_Is_Invalid()
    {
        var model = new CreateCompanyCommand
        {
            Name = "Apple",
            Exchange = "NASDAQ",
            Ticker = "AAPL",
            Isin = "1234567890"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Isin");
    }

    [Fact]
    public void Should_Fail_When_Website_Is_Invalid()
    {
        var model = new CreateCompanyCommand
        {
            Name = "Apple",
            Exchange = "NASDAQ",
            Ticker = "AAPL",
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
        var model = new CreateCompanyCommand
        {
            Name = "Apple",
            Exchange = "NASDAQ",
            Ticker = "AAPL",
            Isin = "US1234567890",
            Website = "https://apple.com"
        };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }
}
