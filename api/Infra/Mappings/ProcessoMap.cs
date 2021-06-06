using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Infra.Mappings
{
    public class ProcessoMap : IEntityTypeConfiguration<Processo>
    {
        public virtual void Configure(EntityTypeBuilder<Processo> builder)
        {
            builder.HasIndex(x => x.NumeroUnificado).IsUnique();

            builder.Property(x => x.DataDistribuicao).IsRequired();
            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.NumeroUnificado).IsRequired().HasMaxLength(20);
            builder.Property(x => x.PastaFisicaCliente).IsRequired().HasMaxLength(50);
        }
    }
}
