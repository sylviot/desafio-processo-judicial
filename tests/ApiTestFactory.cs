using api.Infra;
using api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.TestHost;
using System.Linq;
using AutoMapper;

namespace tests
{
    public class ApiTestFactory : WebApplicationFactory<api.Startup>
    {
        protected IConfiguration configuration;
        public Context context;
        public IMapper mapper;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                this.configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Testing.json")
                    .Build();

                config.AddConfiguration(this.configuration);
            });

            builder.ConfigureServices(services =>
            {
                var serviceContext = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<Context>));
                services.Remove(serviceContext);

                services.AddDbContext<Context>(builder => builder.UseSqlServer(this.configuration["ConnectionsString:AppTest"]));

                this.context = services.BuildServiceProvider().GetRequiredService<Context>();
                this.mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();

                this.ClearContext();
            });
        }

        private void ClearContext()
        {
            try
            {
                foreach(var item in this.context.Processos)
                {
                    item.ProcessoPai = null;
                    this.context.Processos.Update(item);
                }
                this.context.SaveChanges();

                this.context.ProcessoResponsavel.RemoveRange(this.context.ProcessoResponsavel);
                this.context.Responsaveis.RemoveRange(this.context.Responsaveis);
                this.context.Processos.RemoveRange(this.context.Processos);
                this.context.SaveChanges();
            }
            catch
            {
                return;
            }
        }
    }
}
