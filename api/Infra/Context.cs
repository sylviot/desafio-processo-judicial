using api.Infra.Mappings;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Infra
{
    public class Context : DbContext, IDisposable
    {
        private IDbContextTransaction transaction;
        public IDbContextTransaction Transaction
        {
            get
            {
                if (this.transaction == null)
                {
                    this.transaction = this.Database.BeginTransaction();
                }

                return this.transaction;
            }
        }

        public DbSet<Processo> Processos { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }
        public DbSet<ProcessoResponsavel> ProcessoResponsavel { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProcessoMap());
            modelBuilder.ApplyConfiguration(new ResponsavelMap());

            modelBuilder.Entity<ProcessoResponsavel>().HasKey(x => new { x.ProcessoId, x.ResponsavelId });
            modelBuilder.Entity<ProcessoResponsavel>()
                .HasOne(x => x.Responsavel)
                .WithMany(x => x.Processos)
                .HasForeignKey(x => x.ResponsavelId);

            modelBuilder.Entity<ProcessoResponsavel>()
                .HasOne(x => x.Processo)
                .WithMany(x => x.Responsaveis)
                .HasForeignKey(x => x.ProcessoId);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.Transaction;
        }

        public async Task SendChanges()
        {
            await this.Save();
        }

        private async Task CommitAsync()
        {
            if (this.transaction != null)
            {
                await this.transaction.CommitAsync();
                await this.transaction.DisposeAsync();
                this.transaction = null;
            }
        }

        private void Rollback()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.transaction.Dispose();
                this.transaction = null;
            }
        }

        private async Task Save()
        {
            try
            {
                this.ChangeTracker.DetectChanges();
                await this.SaveChangesAsync();
                await this.CommitAsync();
            }
            catch
            {
                this.Rollback();
                throw;
            }
        }
    }
}
