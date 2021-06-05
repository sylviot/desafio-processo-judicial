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
    public class ResponsavelController : ControllerBase
    {
        protected readonly IMapper mapper;
        protected readonly ResponsavelService service;

        public ResponsavelController(IMapper _mapper, ResponsavelService _responsavelService)
        {
            this.mapper = _mapper;
            this.service = _responsavelService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResponsavelDto request)
        {
            var model = this.mapper.Map<Responsavel>(request);
            if(await this.service.CreateAsync(model))
            {
                return Ok(true);
            }

            return BadRequest();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, ResponsavelDto request)
        {
            var model = this.mapper.Map<Responsavel>(request);
            if(await this.service.UpdateAsync(model))
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
            if(await this.service.DeleteAsync(model))
            {
                return Ok(true);
            }

            return BadRequest();
        }
    }
}
