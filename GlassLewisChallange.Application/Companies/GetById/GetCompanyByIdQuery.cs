using MediatR;

namespace GlassLewisChallange.Application.Companies.GetById
{
    public class GetCompanyByIdQuery : IRequest<GetCompanyByIdDto>
    {
        public string Id { get; set; }
    }
}
