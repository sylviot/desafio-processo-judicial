using api.Models.Http;
using api.Services.Interfaces;
using api.Validations;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Validations
{
    public class ProcessoValidatorTests
    {
        protected readonly ProcessoValidator validator;
        protected readonly Mock<IProcessoService> service;
        public ProcessoValidatorTests()
        {
            this.service = new Mock<IProcessoService>();

            this.validator = new ProcessoValidator(this.service.Object);
        }

        [Fact]
        public void Campos_Valido()
        {
            this.service.Setup(x => x.Unique(It.IsAny<ProcessoDto>())).Returns(true);
            var model = new ProcessoDto
            {
                NumeroUnificado = "1234567990000JTR9999",
                DataDistribuicao = DateTime.Now.AddDays(-1),
                Descricao = "Lorem",
                PastaFisicaCliente = "",
                Responsaveis = new[] {
                    new ResponsavelDto { Id = 1 },
                    new ResponsavelDto { Id = 2 },
                    new ResponsavelDto { Id = 3 }
                },
                SegredoJustica = false,
                Situacao = 1
            };

            var result = this.validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void NumeroUnificado_obrigatorio()
        {
            var model = new ProcessoDto { NumeroUnificado = null };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "NumeroUnificado" && x.ErrorCode == "NotEmptyValidator");
        }

        [Fact]
        public void NumeroUnificado_20_caracteres()
        {
            var model = new ProcessoDto { NumeroUnificado = "123" };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "NumeroUnificado" && x.ErrorCode == "ExactLengthValidator");
        }

        [Fact]
        public void NumeroUnificado_formato_validacao()
        {
            var model = new ProcessoDto { NumeroUnificado = "ABC" };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "NumeroUnificado" && x.ErrorCode == "PredicateValidator");
        }

        [Fact]
        public void DataDistribuicao_obrigatorio()
        {
            var model = new ProcessoDto { DataDistribuicao = DateTime.MinValue };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "DataDistribuicao" && x.ErrorCode == "NotEmptyValidator");
        }

        [Fact]
        public void DataDistribuicao_data_futura()
        {
            var model = new ProcessoDto { DataDistribuicao = DateTime.Now.AddDays(1) };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "DataDistribuicao" && x.ErrorCode == "LessThanOrEqualValidator");
        }

        [Fact]
        public void Descricao_limite_1000_caracteres()
        {
            var model = new ProcessoDto { Descricao = new string('a', 1001) };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Descricao" && x.ErrorCode == "MaximumLengthValidator");
        }

        [Fact]
        public void PastaFisicaCliente_obrigatorio()
        {
            var model = new ProcessoDto { PastaFisicaCliente = new string('a', 51) };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "PastaFisicaCliente" && x.ErrorCode == "MaximumLengthValidator");
        }

        [Fact]
        public void Situacao_obrigatorio()
        {
            var model = new ProcessoDto { Situacao = 0 };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Situacao" && x.ErrorCode == "NotEmptyValidator");
        }

        [Fact]
        public void Responsaveis_obrigatorio()
        {
            var model = new ProcessoDto { Responsaveis = null };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Responsaveis" && x.ErrorCode == "NotNullValidator");
        }

        [Fact]
        public void Responsaveis_minimo_1()
        {
            var model = new ProcessoDto { Responsaveis = new ResponsavelDto[] { } };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Responsaveis" && x.ErrorCode == "PredicateValidator");
        }

        [Fact]
        public void Responsaveis_maximo_3()
        {
            var model = new ProcessoDto
            {
                Responsaveis = new[]{
                new ResponsavelDto { Id = 1 },
                new ResponsavelDto { Id = 2 },
                new ResponsavelDto { Id = 3 },
                new ResponsavelDto { Id = 4 }
            }
            };

            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Responsaveis" && x.ErrorCode == "PredicateValidator");
        }
    }
}
