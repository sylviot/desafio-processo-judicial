using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Infra.Mappings
{
    public class ResponsavelMap : IEntityTypeConfiguration<Responsavel>
    {
        public virtual void Configure(EntityTypeBuilder<Responsavel> builder)
        {
            builder.HasIndex(x => x.Cpf).IsUnique();

            builder.Property(x => x.Cpf).IsRequired().HasMaxLength(14);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(400);
            builder.Property(x => x.Foto).IsRequired();
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(150);
        }
    }
}
