using AutoMapper;
using GlassLewisChallange.Application.Exceptions;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GlassLewisChallange.Application.Companies.GetById
{
    public class GetCompanyByIdHandler : IRequestHandler<GetCompanyByIdQuery, GetCompanyByIdDto>
    {
        private readonly IGlassLewisContext _context;
        private readonly IMapper _mapper;
        private readonly IIdProtector _protector;

        public GetCompanyByIdHandler(IGlassLewisContext context, IMapper mapper, IIdProtector protector)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
        }

        public async Task<GetCompanyByIdDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            Guid decryptedId;
            try
            {
                decryptedId = _protector.Unprotect(request.Id);
            }
            catch
            {
                throw new ArgumentException("Invalid ID format.");
            }

            var entity = await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == decryptedId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Company", request.Id);
            }

            var dto = _mapper.Map<GetCompanyByIdDto>(entity);
            dto.Id = request.Id;

            return dto;
        }
    }
}
