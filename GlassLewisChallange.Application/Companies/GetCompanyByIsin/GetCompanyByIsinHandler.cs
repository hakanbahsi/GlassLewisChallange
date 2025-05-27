using AutoMapper;
using GlassLewisChallange.Application.Exceptions;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GlassLewisChallange.Application.Companies.GetCompanyByIsin
{
    public class GetCompanyByIsinHandler : IRequestHandler<GetCompanyByIsinQuery, GetCompanyByIsinDto>
    {
        private readonly IGlassLewisContext _context;
        private readonly IMapper _mapper;
        private readonly IIdProtector _protector;

        public GetCompanyByIsinHandler(IGlassLewisContext context, IMapper mapper, IIdProtector protector)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
        }

        public async Task<GetCompanyByIsinDto> Handle(GetCompanyByIsinQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Isin == request.Isin, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Company", request.Isin);
            }

            var dto = _mapper.Map<GetCompanyByIsinDto>(entity);
            dto.Id = _protector.Protect(entity.Id);

            return dto;
        }
    }
}
