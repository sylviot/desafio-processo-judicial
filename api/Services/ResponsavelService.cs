using api.Infra;
using api.Models;
using api.Models.Http;
using api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class ResponsavelService : ServiceBase<Responsavel>, IResponsavelService
    {
        public ResponsavelService(Context _context)
            : base(_context)
        {
        }
        public bool Unique(ResponsavelDto responsavel)
        {
            return !this.Read().Any(x => x.Cpf == responsavel.Cpf && x.Id != responsavel.Id);
        }
    }
}
