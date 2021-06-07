using api.Infra;
using api.Models;
using api.Models.Http;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class ProcessoService : ServiceBase<Processo>, IProcessoService
    {
        public ProcessoService(Context _context)
            : base(_context)
        {
        }

        public int Deep(int? paiId, int deep = 0)
        {
            if(!paiId.HasValue || deep > 4)
            {
                return deep;
            }

            var pai = this.Read().SingleOrDefault(x => x.Id == paiId);
            return Deep(pai?.ProcessoPai, deep + 1);
        }

        public bool Exists(int id)
        {
            return this.Read().Any(x => x.Id == id);
        }

        public bool Leef(int target, int id)
        {
            var filhos = this.Read().Where(x => x.ProcessoPai == id).Select(x => x.Id).ToList();
            if (filhos.Any(x => x == target))
            {
                return false;
            }

            foreach (var item in filhos)
            {
                return Leef(target, item);
            }

            return false;
        }

        public bool Unique(ProcessoDto processo)
        {
            return !this.Read().Any(x => x.NumeroUnificado == processo.NumeroUnificado && x.Id != processo.Id);
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
