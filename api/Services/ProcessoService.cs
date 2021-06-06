using api.Infra;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public override Task<bool> UpdateAsync(Processo entity)
        {
            base.context.BeginTransaction();

            var responsaveis = base.context.ProcessoResponsavel.Where(x => x.ProcessoId == entity.Id).ToList();
            base.context.ProcessoResponsavel.RemoveRange(responsaveis);
            base.context.ProcessoResponsavel.AddRangeAsync(entity.Responsaveis);

            return base.UpdateAsync(entity);
        }

        public override Task<bool> DeleteAsync(Processo entity)
        {
            var responsaveis = base.context.ProcessoResponsavel.Where(x => x.ProcessoId == entity.Id).ToList();
            base.context.ProcessoResponsavel.RemoveRange(responsaveis);

            return base.DeleteAsync(entity);
        }
    }
}
