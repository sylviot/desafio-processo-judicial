using api.Models.Http;
using api.Services;
using FluentValidation;
using System.Text.RegularExpressions;

namespace api.Validations
{
    public class ResponsavelValidator : AbstractValidator<ResponsavelDto>
    {
        protected readonly ResponsavelService service;
        public ResponsavelValidator(ResponsavelService _responsavelService)
        {
            this.service = _responsavelService;

            RuleFor(x => x)
                .Must(this.service.Unique).WithMessage("CPF já cadastrado");

            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("Campo obrigatório")
                .Length(14).WithMessage("Necessário 14 caracteres")
                .Must(this.CpfValidation).WithMessage("CPF inválido")
                .Must(this.CpfFormatValidation).WithMessage("CPF no formato inválido");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Campo obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MaximumLength(400).WithMessage("Limite de 400 caracteres");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Campo obrigatório")
                .MaximumLength(150).WithMessage("Limite de 150 caracteres");
        }

        public bool CpfFormatValidation(string cpf)
        {
            return Regex.IsMatch(cpf, @"\d{3}\.\d{3}\.\d{3}\-\d{2}");
        }

        public bool CpfValidation(string cpf)
        {
            return true;
        }
    }
}
