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
        int Deep(int? paiId, int deep = 0);
        bool Leef(int target, int id);
        bool Unique(ProcessoDto processo);
    }
}
