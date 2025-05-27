using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassLewisChallange.Application.Companies.GetCompanyByIsin
{
    public class GetCompanyByIsinValidator : AbstractValidator<GetCompanyByIsinQuery>
    {
        public GetCompanyByIsinValidator()
        {
            RuleFor(x => x.Isin)
                .NotEmpty().WithMessage("ISIN is required.")
                .MinimumLength(2).WithMessage("ISIN must be at least 2 characters.")
                .Must(i => char.IsLetter(i[0]) && char.IsLetter(i[1]))
                .WithMessage("ISIN must start with two letters.");
        }
    }
}
