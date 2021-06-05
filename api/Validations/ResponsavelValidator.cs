using api.Models.Http;
using api.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Validations
{
    public class ResponsavelValidator : AbstractValidator<ResponsavelDto>
    {
        public ResponsavelValidator()
        {
            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("Campo obrigatório")
                .Length(14).WithMessage("Necessário 14 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Campo obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MaximumLength(400).WithMessage("Limite de 400 caracteres");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Campo obrigatório")
                .MaximumLength(150).WithMessage("Limite de 150 caracteres");
        }
    }
}
