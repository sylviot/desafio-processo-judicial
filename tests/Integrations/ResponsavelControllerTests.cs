using api.Infra;
using api.Models;
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
using Microsoft.Extensions.DependencyInjection;
using api.Models.Http;

namespace Integrations
{
    public class ResponsavelControllerTests : IClassFixture<ApiTestFactory>
    {
        protected readonly ApiTestFactory fixture;
        protected readonly HttpClient client;
        public ResponsavelControllerTests(ApiTestFactory fixture)
        {
            this.fixture = fixture;
            this.client = this.fixture.CreateClient();
        }

        [Fact]
        public async Task Todos()
        {
            var response = await this.client.GetAsync("/Responsavel/All");

            var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(data.data);
        }

        [Fact]
        public async Task Criar()
        {
            var data = new ResponsavelDto { Cpf = "000.000.000-00", Email = "email@email.com", Foto = "base64;", Nome = "João da Silva" };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            
            var response = await this.client.PostAsync("/Responsavel/Create", content);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Editar()
        {
            var data = new ResponsavelDto { Cpf = "000.111.111-11", Email = "email@email.com", Foto = "base64;", Nome = "João da Silva" };
            var contentResponsavel = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var responseResponsavel = await this.client.PostAsync("/Responsavel/Create", contentResponsavel);
            Assert.True(responseResponsavel.IsSuccessStatusCode);

            var responsavel = JsonConvert.DeserializeObject<Responsavel>(await responseResponsavel.Content.ReadAsStringAsync());

            data.Id = responsavel.Id;
            data.Email = "update@email.com";
            data.Foto = "base64,update;";
            data.Nome = "Update da Silva";

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Patch, $"/Responsavel/Update/{responsavel.Id}");
            requestMessage.Content = content;
            var response = await this.client.SendAsync(requestMessage);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Deletar()
        {
            var data = new ResponsavelDto { Cpf = "000.222.222-22", Email = "email@email.com", Foto = "base64;", Nome = "João da Silva" };
            var contentResponsavel = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var responseResponsavel = await this.client.PostAsync("/Responsavel/Create", contentResponsavel);
            Assert.True(responseResponsavel.IsSuccessStatusCode);

            var responsavel = JsonConvert.DeserializeObject<Responsavel>(await responseResponsavel.Content.ReadAsStringAsync());

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/Responsavel/Delete/{responsavel.Id}");
            var response = await this.client.SendAsync(requestMessage);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
