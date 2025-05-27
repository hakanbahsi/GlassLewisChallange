using FluentAssertions;
using GlassLewisChallange.Application.Companies.GetCompanyByIsin;

namespace GlassLewisChallange.Tests.Application.Validators;

public class GetCompanyByIsinValidatorTests
{
    private readonly GetCompanyByIsinValidator _validator;

    public GetCompanyByIsinValidatorTests()
    {
        _validator = new GetCompanyByIsinValidator();
    }

    [Fact]
    public void Should_Pass_When_Isin_Is_Provided()
    {
        var model = new GetCompanyByIsinQuery { Isin = "US1234567890" };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }
}
