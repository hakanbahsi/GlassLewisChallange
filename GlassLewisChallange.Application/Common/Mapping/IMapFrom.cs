﻿using AutoMapper;

namespace GlassLewisChallange.Application.Common.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }
    }
}
