using FluentAssertions;
using GlassLewisChallange.Application.Companies.GetById;
using GlassLewisChallange.Infrastructure.Security;
using Moq;

namespace GlassLewisChallange.Tests.Application.Validators;

public class GetCompanyByIdValidatorTests
{
    private readonly GetCompanyByIdValidator _validator;

    public GetCompanyByIdValidatorTests()
    {
        var protectorMock = new Mock<IIdProtector>();

        protectorMock.Setup(p => p.CanUnprotect("valid-id")).Returns(true);
        protectorMock.Setup(p => p.CanUnprotect("invalid-id")).Returns(false);

        _validator = new GetCompanyByIdValidator(protectorMock.Object);
    }

    [Fact]
    public void Should_Fail_When_Id_Cannot_Be_Decrypted()
    {
        var model = new GetCompanyByIdQuery { Id = "invalid-id" };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_Pass_When_Id_Can_Be_Decrypted()
    {
        var model = new GetCompanyByIdQuery { Id = "valid-id" };

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
    }
}
