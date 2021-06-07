using api.Models;
using api.Models.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tests;
using Xunit;

namespace Integrations
{
    public class ProcessoControllerTests : IClassFixture<ApiTestFactory>
    {
        protected readonly ApiTestFactory fixture;
        protected readonly HttpClient client;
        public ProcessoControllerTests(ApiTestFactory fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.CreateClient();
        }

        [Fact]
        public async Task Todos()
        {
            var response = await this.client.GetAsync("/Processo/All");

            var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(this.fixture.context.Processos.Count(), data.data.Count);
        }

        [Fact]
        public async Task Criar()
        {
            var dataResponsavel = new ResponsavelDto { Cpf = "111.111.111-11", Email = "email@email.com", Foto = "base64;", Nome = "João da Silva" };
            var responseResponsavelCreate = await this.client.PostAsync("/Responsavel/Create", new StringContent(JsonConvert.SerializeObject(dataResponsavel), Encoding.UTF8, "application/json"));
            Assert.True(responseResponsavelCreate.IsSuccessStatusCode);

            var responsavel = JsonConvert.DeserializeObject<Responsavel>(await responseResponsavelCreate.Content.ReadAsStringAsync());

            var numeroUnificado = "1234567990000JTR9999";
            var data = new ProcessoDto
            {
                ProcessoPai = null,
                NumeroUnificado = numeroUnificado,
                DataDistribuicao = DateTime.Now.AddDays(-1),
                Descricao = "Lorem",
                PastaFisicaCliente = "//pasta/client",
                SegredoJustica = false,
                Situacao = 1,
                Responsaveis = new ResponsavelDto[]
                {
                    new ResponsavelDto { Id = responsavel.Id }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync("/Processo/Create", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Editar()
        {
            var dataResponsavel = new ResponsavelDto { Cpf = "222.222.222-22", Email = "email@email.com", Foto = "base64;", Nome = "João da Silva" };
            var responseResponsavelCreate = await this.client.PostAsync("/Responsavel/Create", new StringContent(JsonConvert.SerializeObject(dataResponsavel), Encoding.UTF8, "application/json"));
            Assert.True(responseResponsavelCreate.IsSuccessStatusCode);

            var responsavel = JsonConvert.DeserializeObject<Responsavel>(await responseResponsavelCreate.Content.ReadAsStringAsync());
            
            var data = new ProcessoDto
            {
                NumeroUnificado = "2222222990000JTR9999",
                DataDistribuicao = DateTime.Now.AddDays(-1).Date,
                Descricao = "Lorem",
                PastaFisicaCliente = "//pasta/client",
                SegredoJustica = false,
                Situacao = 1,
                Responsaveis = new ResponsavelDto[]
                {
                    new ResponsavelDto { Id = responsavel.Id }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var responseCreate = await this.client.PostAsync("/Processo/Create", content);
            Assert.True(responseCreate.IsSuccessStatusCode);

            var created = JsonConvert.DeserializeObject<Processo>(await responseCreate.Content.ReadAsStringAsync());
            data.Id = created.Id;

            var requestMessage = new HttpRequestMessage(HttpMethod.Patch, $"/Processo/Update/{created.Id}");
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
            var response = await this.client.SendAsync(requestMessage);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Deletar()
        {
            var dataResponsavel = new ResponsavelDto { Cpf = "333.333.333-33", Email = "email@email.com", Foto = "base64;", Nome = "João da Silva" };
            var responseResponsavelCreate = await this.client.PostAsync("/Responsavel/Create", new StringContent(JsonConvert.SerializeObject(dataResponsavel), Encoding.UTF8, "application/json"));
            Assert.True(responseResponsavelCreate.IsSuccessStatusCode);

            var responsavel = JsonConvert.DeserializeObject<Responsavel>(await responseResponsavelCreate.Content.ReadAsStringAsync());

            var data = new ProcessoDto
            {
                NumeroUnificado = "3333333990000JTR9999",
                DataDistribuicao = DateTime.Now.AddDays(-1).Date,
                Descricao = "Lorem",
                PastaFisicaCliente = "//pasta/client",
                SegredoJustica = false,
                Situacao = 1,
                Responsaveis = new ResponsavelDto[]
                {
                    new ResponsavelDto { Id = responsavel.Id }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var responseCreate = await this.client.PostAsync("/Processo/Create", content);
            Assert.True(responseCreate.IsSuccessStatusCode);
            
            var created = JsonConvert.DeserializeObject<Processo>(await responseCreate.Content.ReadAsStringAsync());

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/Processo/Delete/{created.Id}");
            var response = await this.client.SendAsync(requestMessage);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
