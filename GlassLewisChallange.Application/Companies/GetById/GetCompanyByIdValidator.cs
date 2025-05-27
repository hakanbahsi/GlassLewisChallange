using FluentValidation;
using GlassLewisChallange.Infrastructure.Security;

namespace GlassLewisChallange.Application.Companies.GetById
{
    public class GetCompanyByIdValidator : AbstractValidator<GetCompanyByIdQuery>
    {
        public GetCompanyByIdValidator(IIdProtector protector)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(id => protector.CanUnprotect(id))
                .WithMessage("Invalid or corrupted Id.");
        }
    }
}
