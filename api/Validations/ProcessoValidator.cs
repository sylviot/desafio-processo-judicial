using api.Models.Http;
using api.Services;
using api.Services.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

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
        protected readonly IProcessoService service;
        public ProcessoValidator(IProcessoService _processoService)
        {
            this.service = _processoService;
            RuleFor(x => x)
                .Must(this.service.Unique).WithMessage("Número unificado já cadastrado")
                .Must(this.ProcessoNaoCiclico).WithMessage("ProcessoPai está contido na árvore");

            RuleFor(x => x.ProcessoPai)
                .Must(x => !x.HasValue || this.service.Exists(x.Value)).WithMessage("ProcessoPai não existe")
                .Must(x => this.service.Deep(x) < 4).WithMessage("ProcessoPai já está no 4º nível (Neto)");

            RuleFor(x => x.NumeroUnificado)
                .NotEmpty().WithMessage("Campo obrigatório")
                .Length(20).WithMessage("Número unificado tem 20 caracteres")
                .Must(this.NumeroUnificadoFormatValidation).WithMessage("Número unificado inválido");

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
                .Must(x => x != null && x.Count() >= 1).WithMessage("Minímo de 1 responsável")
                .Must(x => x != null && x.Count() <= 3).WithMessage("Limite de 3 responsáveis")
                .Must((root, c) => c != null && c.Distinct(new ComparadorResponsavel()).Count() == root.Responsaveis.Count()).WithMessage("Escolha responsáveis distintos.");
        }

        public bool NumeroUnificadoFormatValidation(string numeroUnificado)
        {
            if(string.IsNullOrEmpty(numeroUnificado))
            {
                return false;
            }

            return Regex.IsMatch(numeroUnificado, @"\d{7}\d{2}\d{4}\w{3}\d{4}");
        }

        public bool ProcessoNaoCiclico(ProcessoDto processo)
        {
            if (processo.ProcessoPai.HasValue && processo.Id > 0)
            {
                return this.service.Leef(processo.ProcessoPai.Value, processo.Id);
            }

            return true;
        }
    }
}
