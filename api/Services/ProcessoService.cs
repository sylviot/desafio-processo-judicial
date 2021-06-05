using api.Infra;
using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class ProcessoService : ServiceBase<Processo>
    {
        public ProcessoService(Context _context)
            : base(_context)
        {
        }
    }
}
