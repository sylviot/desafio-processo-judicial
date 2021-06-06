using api.Models.Http;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace api.Validations
{
    public class ComparadorResponsavel : IEqualityComparer<ResponsavelDto>
    {
        public bool Equals(ResponsavelDto x, ResponsavelDto y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] ResponsavelDto obj)
        {
            return obj.Id;
        }
    }
    public class ProcessoValidator : AbstractValidator<ProcessoDto>
    {
        public ProcessoValidator()
        {
            RuleFor(x => x.NumeroUnificado)
                .NotEmpty().WithMessage("Campo obrigatório")
                .Length(20).WithMessage("Número unificado invalido");

            RuleFor(x => x.DataDistribuicao)
                .NotEmpty().WithMessage("Campo obrigatório")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Data no tempo futuro");

            RuleFor(x => x.Descricao)
                .MaximumLength(1000).WithMessage("Limite de 1000 caracteres");

            RuleFor(x => x.PastaFisicaCliente)
                .MaximumLength(50).WithMessage("Limite de 50 caracteres");

            RuleFor(x => x.Situacao)
                .NotEmpty().WithMessage("Campo obrigatório");

            RuleFor(x => x.Responsaveis)
                .NotNull().WithMessage("Escolha algum responsável")
                .Must(x => x.Count() <= 3).WithMessage("Limite de 3 responsáveis")
                .Must((root, c) => c.Distinct(new ComparadorResponsavel()).Count() == root.Responsaveis.Count()).WithMessage("Escolha responsáveis distintos.");
        }
    }
}
