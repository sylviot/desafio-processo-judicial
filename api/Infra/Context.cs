using api.Infra.Mappings;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Infra
{
    public class Context : DbContext
    {
        public DbSet<Processo> Processos { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProcessoMap());
            modelBuilder.ApplyConfiguration(new ResponsavelMap());
        }
    }
}
