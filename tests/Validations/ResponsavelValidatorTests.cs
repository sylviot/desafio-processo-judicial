using api.Models.Http;
using api.Services.Interfaces;
using api.Validations;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Validations
{
    public class ResponsavelValidatorTests
    {
        protected readonly ResponsavelValidator validator;
        protected readonly Mock<IResponsavelService> service;
        public ResponsavelValidatorTests()
        {
            this.service = new Mock<IResponsavelService>();

            this.validator = new ResponsavelValidator(this.service.Object);
        }

        [Fact]
        public void Campos_Valido()
        {
            this.service.Setup(x => x.Unique(It.IsAny<ResponsavelDto>())).Returns(true);
            var model = new ResponsavelDto
            {
                Cpf = "000.000.000-00",
                Email = "email@email.com",
                Foto = "base64,123",
                Nome = "João da Silva"
            };

            var result = this.validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Cpf_Unico()
        {
            this.service.Setup(x => x.Unique(It.IsAny<ResponsavelDto>())).Returns(false);
            var model = new ResponsavelDto();

            var result = this.validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void Cpf_obrigatorio()
        {
            var model = new ResponsavelDto() { Cpf = null };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Cpf" && x.ErrorCode == "NotEmptyValidator");
        }

        [Fact]
        public void Cpf_14_caracteres()
        {
            var model = new ResponsavelDto() { Cpf = "123" };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Cpf" && x.ErrorCode == "ExactLengthValidator");
        }

        [Fact]
        public void Cpf_formato_validacao()
        {
            var model = new ResponsavelDto() { Cpf = "000-000-000.00" };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Cpf" && x.ErrorCode == "PredicateValidator");
        }

        [Fact]
        public void Email_obrigatorio()
        {
            var model = new ResponsavelDto() { Email = null };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Email" && x.ErrorCode == "NotEmptyValidator");
        }

        [Fact]
        public void Email_validacao()
        {
            var model = new ResponsavelDto() { Email = "email.com" };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Email" && x.ErrorCode == "EmailValidator");
        }

        [Fact]
        public void Email_limite_400_caracteres()
        {
            var model = new ResponsavelDto() { Email = new string('e', 400) + "@email.com" };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Email" && x.ErrorCode == "MaximumLengthValidator");
        }

        [Fact]
        public void Nome_obrigatorio()
        {
            var model = new ResponsavelDto() { Nome = null };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Nome" && x.ErrorCode == "NotEmptyValidator");
        }

        [Fact]
        public void Nome_limite_150_caracteres()
        {
            var model = new ResponsavelDto() { Nome = "João da Silva" + new string('a', 150) };
            var result = this.validator.TestValidate(model);
            Assert.Contains(result.Errors, x => x.PropertyName == "Nome" && x.ErrorCode == "MaximumLengthValidator");
        }
    }
}
