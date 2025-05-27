using AutoMapper;
using GlassLewisChallange.Application.Common.Mapping;
using GlassLewisChallange.Domain.Entities;
using MediatR;

namespace GlassLewisChallange.Application.Companies.Create
{
    public class CreateCompanyCommand : IRequest<CreateCompanyDto>, IMapFrom<Company>
    {
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string? Website { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCompanyCommand, Company>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
