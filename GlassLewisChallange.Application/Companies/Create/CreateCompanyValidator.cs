using FluentValidation;

namespace GlassLewisChallange.Application.Companies.Create
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Exchange)
                .NotEmpty().WithMessage("Exchange is required.");

            RuleFor(x => x.Ticker)
                .NotEmpty().WithMessage("Ticker is required.");

            RuleFor(x => x.Isin)
                .NotEmpty().WithMessage("ISIN is required.")
                .MinimumLength(2).WithMessage("ISIN must be at least 2 characters.")
                .Must(i => char.IsLetter(i[0]) && char.IsLetter(i[1]))
                .WithMessage("ISIN must start with two letters.");

            RuleFor(x => x.Website)
                .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Website must be a valid URL if provided.");
        }
    }
}
