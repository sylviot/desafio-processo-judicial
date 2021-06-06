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

namespace tests
{
    public class ApiTestFactory : WebApplicationFactory<api.Startup>
    {
        protected IConfiguration configuration;
        public Context context;
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
                //db.Database.EnsureCreated();
            });

            //base.ConfigureWebHost(builder);
        }
    }
}
