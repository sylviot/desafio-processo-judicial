using api.Models;
using api.Models.Http;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Responsavel, ResponsavelDto>();
            CreateMap<ResponsavelDto, Responsavel>();
        }
    }
}
