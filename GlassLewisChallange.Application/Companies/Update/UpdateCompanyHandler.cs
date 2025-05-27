using GlassLewisChallange.Application.Exceptions;
using GlassLewisChallange.Domain.Interfaces;
using GlassLewisChallange.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GlassLewisChallange.Application.Companies.Update
{
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, Unit>
    {
        private readonly IGlassLewisContext _context;
        private readonly IIdProtector _protector;

        public UpdateCompanyHandler(IGlassLewisContext context, IIdProtector protector)
        {
            _context = context;
            _protector = protector;
        }

        public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var decryptedId = _protector.Unprotect(request.Id);

            var entity = await _context.Companies.FirstOrDefaultAsync(c => c.Id == decryptedId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Company", request.Id);
            }

            entity.Update(request.Name, request.Exchange, request.Ticker, request.Isin, request.Website);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
