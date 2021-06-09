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
    public class ResponsavelController : ControllerBase
    {
        protected readonly IMapper mapper;
        protected readonly IResponsavelService service;

        public ResponsavelController(IMapper _mapper, IResponsavelService _responsavelService)
        {
            this.mapper = _mapper;
            this.service = _responsavelService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] ResponsavelFilterDto request)
        {
            var query = await this.service.Read()
                .Where(x => string.IsNullOrEmpty(request.Cpf) || x.Cpf.Replace(".","").Replace("-", "").Contains(request.Cpf.Replace(".", "").Replace("-", "")))
                .Where(x => string.IsNullOrEmpty(request.Nome) || x.Nome.Contains(request.Nome))
                .Skip((int)request.Size * ((int)request.Page - 1))
                .Take((int)request.Size + 1)
                .ToListAsync();
            int? next = null;
            int? previous = null;

            if(query.Count > request.Size)
            {
                next = (int)request.Page + 1;
            }

            if(request.Page > 1)
            {
                previous = (int)request.Page - 1;
            }

            return Ok(new { data = query, page = request.Page, size = request.Size, previous = previous, next = next });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResponsavelDto request)
        {
            var model = this.mapper.Map<Responsavel>(request);
            if (await this.service.CreateAsync(model) != null)
            {
                return Ok(model);
            }

            return BadRequest();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, ResponsavelDto request)
        {
            var model = this.mapper.Map<Responsavel>(request);
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
            var model = new Responsavel { Id = id };
            if (await this.service.DeleteAsync(model))
            {
                return Ok(true);
            }

            return BadRequest();
        }
    }
}
