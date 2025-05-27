using AutoMapper;
using GlassLewisChallange.Domain.Entities;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GlassLewisChallange.Application.Companies.Create
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyDto>
    {
        private readonly IGlassLewisContext _context;
        private readonly IMapper _mapper;
        private readonly IIdProtector _protector;

        public CreateCompanyHandler(
            IGlassLewisContext context,
            IMapper mapper,
            IIdProtector protector)
        {
            _context = context;
            _mapper = mapper;
            _protector = protector;
        }

        public async Task<CreateCompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Company>(request);

            var isinExists = await _context.Companies
                .AnyAsync(x => x.Isin == entity.Isin, cancellationToken);

            if (isinExists)
            {
                throw new InvalidOperationException("A company with the same ISIN already exists.");
            }

            _context.Companies.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCompanyDto
            {
                Id = _protector.Protect(entity.Id)
            };
        }
    }

}
