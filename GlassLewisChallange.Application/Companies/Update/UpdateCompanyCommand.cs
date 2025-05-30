﻿using MediatR;

namespace GlassLewisChallange.Application.Companies.Update
{
    public class UpdateCompanyCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string? Website { get; set; }
    }
}
