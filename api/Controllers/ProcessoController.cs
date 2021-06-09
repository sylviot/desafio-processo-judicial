using api.Models;
using api.Models.Http;
using api.Services;
using api.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProcessoController : ControllerBase
    {
        protected readonly IMapper mapper;
        protected readonly IProcessoService service;

        public ProcessoController(IMapper _mapper, IProcessoService _processoService)
        {
            this.mapper = _mapper;
            this.service = _processoService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] ResponsavelFilterDto request)
        {
            var query = await this.service.Read()
                .Include("Responsaveis.Responsavel")
                .Include(x => x.Situacao)
                .Where(x => string.IsNullOrEmpty(request.Nome) || x.NumeroUnificado.Contains(request.Nome))
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

            return Ok(new { data = query, page = request.Page, size = request.Size, previous = previous, next = next });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProcessoDto request)
        {
            var model = this.mapper.Map<Processo>(request);
            if(await this.service.CreateAsync(model) != null)
            {
                return Ok(model);
            }

            return BadRequest();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update(ProcessoDto request)
        {
            var model = this.mapper.Map<Processo>(request);
            //var model = this.service.Read().FirstOrDefault(x => x.Id == request.Id);
            if (await this.service.UpdateAsync(model))
            {
                return Ok(true);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = new Processo { Id = id };
            if(await this.service.DeleteAsync(model))
            {
                return Ok(true);
            }

            return BadRequest();
        }
    }
}
