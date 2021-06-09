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
        protected readonly IMailService mailService;

        public ProcessoController(IMapper _mapper, IProcessoService _processoService, IMailService _mailService)
        {
            this.mapper = _mapper;
            this.service = _processoService;
            this.mailService = _mailService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] ProcessoFilterDto request)
        {
            try
            {
                return Ok(await this.service.Paginate(request));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProcessoDto request)
        {
            var model = this.mapper.Map<Processo>(request);
            if(await this.service.CreateAsync(model) != null)
            {
                foreach(var item in model.Responsaveis)
                {
                    this.mailService.Enviar(item.Responsavel.Email, $"Processo {model.NumeroUnificado} - Envolvido", $"Você foi cadastrado como envolvido no processo de número {model.NumeroUnificado}");
                }

                return Ok(model);
            }

            return BadRequest();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update(ProcessoDto request)
        {
            var model = this.mapper.Map<Processo>(request);
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
