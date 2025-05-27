using GlassLewisChallange.Application.Common.Mapping;
using GlassLewisChallange.Domain.Entities;

namespace GlassLewisChallange.Application.Companies.GetById
{
    public class GetCompanyByIdDto : IMapFrom<Company>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string? Website { get; set; }
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Company, GetCompanyByIdDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
}
