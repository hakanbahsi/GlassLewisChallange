using MediatR;

namespace GlassLewisChallange.Application.Companies.GetCompanyByIsin
{
    public class GetCompanyByIsinQuery : IRequest<GetCompanyByIsinDto>
    {
        public string Isin { get; set; }
    }
}
