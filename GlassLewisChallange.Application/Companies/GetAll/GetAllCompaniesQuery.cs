using MediatR;

namespace GlassLewisChallange.Application.Companies.GetAll
{
    public class GetAllCompaniesQuery : IRequest<List<GetAllCompaniesDto>>
    {
    }
}
