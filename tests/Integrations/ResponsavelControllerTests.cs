using api.Models;
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
    public class ResponsavelControllerTests : IClassFixture<ApiTestFactory>
    {
        protected readonly HttpClient client;
        public ResponsavelControllerTests(ApiTestFactory fixture)
        {
            this.client = fixture.CreateClient();
        }

        [Fact]
        public async Task All()
        {
            var response = await this.client.GetAsync("/Responsavel/All");

            var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            Assert.Equal(2, data.data.Count);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
