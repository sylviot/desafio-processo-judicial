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

        public async Task<DataPaginationDto> Paginate(ProcessoFilterDto request)
        {
            var query = await this.Read()
                .Include("Responsaveis.Responsavel")
                .Include(x => x.Situacao)
                .Where(x => string.IsNullOrEmpty(request.NumeroUnificado) || x.NumeroUnificado.Contains(request.NumeroUnificado))
                .Where(x => !request.DataDistribuicaoInicio.HasValue || x.DataDistribuicao > request.DataDistribuicaoInicio.Value)
                .Where(x => !request.DataDistribuicaoFim.HasValue || x.DataDistribuicao < request.DataDistribuicaoFim.Value)
                .Where(x => !request.SituacaoId.HasValue || x.SituacaoId == request.SituacaoId.Value)
                .Where(x => !request.SegredoJustica.HasValue || x.SegredoJustica == request.SegredoJustica.Value)
                .Where(x => string.IsNullOrEmpty(request.PastaFisicaCliente) || x.PastaFisicaCliente.Contains(request.PastaFisicaCliente))
                .Where(x => string.IsNullOrEmpty(request.Responsavel) || x.Responsaveis.Any(a => a.Responsavel.Nome.Contains(request.Responsavel)))
                .Skip((int)request.Size * ((int)request.Page - 1))
                .Take((int)request.Size + 1)
                .ToListAsync();

            int? next = null;
            int? previous = null;

            if (query.Count() > request.Size)
            {
                next = (int)request.Page + 1;
            }

            if (request.Page > 1)
            {
                previous = (int)request.Page - 1;
            }

            query = query.Take((int)request.Size).ToList();

            return new DataPaginationDto { data = query, page = request.Page, size = request.Size, previous = previous, next = next };
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

        public override async Task<Processo> CreateAsync(Processo entity)
        {
            await base.CreateAsync(entity);
            return await this.Read().Include("Responsaveis.Responsavel").FirstOrDefaultAsync(x => x.Id == entity.Id);
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
