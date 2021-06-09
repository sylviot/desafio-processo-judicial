using api.Models;
using api.Models.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.Interfaces
{
    public interface IProcessoService : IServiceBase<Processo>
    {
        Task<DataPaginationDto> Paginate(ProcessoFilterDto request);
        int Deep(int? paiId, int deep = 0);
        bool Exists(int id);
        bool Leef(int target, int id, int deep = 0);
        bool Unique(ProcessoDto processo);
    }
}
