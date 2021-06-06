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
        public async Task All()
        {
            var response = await this.client.GetAsync("/Responsavel/All");

            var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(this.fixture.context.Responsaveis.Count(), data.data.Count);
        }
    }
}
