using AutoMapper;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GlassLewisChallange.Application.Companies.GetAll
{
    public class GetAllCompaniesHandler : IRequestHandler<GetAllCompaniesQuery, List<GetAllCompaniesDto>>
    {
        private readonly IGlassLewisContext _context;
        private readonly IMapper _mapper;
        private readonly IIdProtector _protector;

        public GetAllCompaniesHandler(IGlassLewisContext context, IMapper mapper, IIdProtector protector)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
        }

        public async Task<List<GetAllCompaniesDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _context.Companies
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<List<GetAllCompaniesDto>>(companies);

            foreach (var company in result)
            {
                var original = companies.First(c => c.Isin == company.Isin);
                company.Id = _protector.Protect(original.Id);
            }

            return result;
        }
    }
}
