using api.Models;
using api.Models.Http;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        protected readonly ProcessoService service;

        public ProcessoController(IMapper _mapper, ProcessoService _processoService)
        {
            this.mapper = _mapper;
            this.service = _processoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProcessoDto request)
        {
            var model = this.mapper.Map<Processo>(request);
            if(await this.service.CreateAsync(model))
            {
                return Ok(true);
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
