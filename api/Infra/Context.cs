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
    public class Context : DbContext
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

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProcessoMap());
            modelBuilder.ApplyConfiguration(new ResponsavelMap());
        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.Transaction;
        }

        public void SendChanges()
        {
            this.Save();
            this.Commit();
        }

        private void Commit()
        {
            if(this.transaction != null)
            {
                this.transaction.Commit();
                this.transaction.Dispose();
                this.transaction = null;
            }
        }

        private void Rollback()
        {
            if(this.transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        private void Save()
        {
            try
            {
                this.ChangeTracker.DetectChanges();
                this.SaveChanges();
            }
            catch
            {
                this.Rollback();
            }
        }
    }
}
