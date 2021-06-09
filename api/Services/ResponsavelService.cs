using api.Infra;
using api.Models;
using api.Models.Http;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<DataPaginationDto> Paginate(ResponsavelFilterDto request)
        {
            var query = await this.Read()
                .Where(x => string.IsNullOrEmpty(request.Cpf) || x.Cpf.Replace(".", "").Replace("-", "").Contains(request.Cpf.Replace(".", "").Replace("-", "")))
                .Where(x => string.IsNullOrEmpty(request.Nome) || x.Nome.Contains(request.Nome))
                .Skip((int)request.Size * ((int)request.Page - 1))
                .Take((int)request.Size + 1)
                .ToListAsync();

            int? next = null;
            int? previous = null;

            if (query.Count > request.Size)
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

        public bool Unique(ResponsavelDto responsavel)
        {
            return !this.Read().Any(x => x.Cpf == responsavel.Cpf && x.Id != responsavel.Id);
        }
    }
}
