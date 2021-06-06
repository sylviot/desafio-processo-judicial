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

            CreateMap<Processo, ProcessoDto>();
            CreateMap<ProcessoDto, Processo>()
                .ForMember(x => x.Responsaveis, x => x.MapFrom((pdto, p) => pdto.Responsaveis.Select(s => new ProcessoResponsavel { ProcessoId = pdto.Id, ResponsavelId = s.Id })));
        }
    }
}
