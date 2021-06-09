using api.Models;
using api.Models.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.Interfaces
{
    public interface IResponsavelService : IServiceBase<Responsavel>
    {
        Task<DataPaginationDto> Paginate(ResponsavelFilterDto request);
        bool Unique(ResponsavelDto responsavel);
    }
}
