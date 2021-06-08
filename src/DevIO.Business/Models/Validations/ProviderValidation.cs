using DevIO.Business.Models.Validations.Docs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Models.Validations
{
    public class ProviderValidation : AbstractValidator<Provider>
    {
        public ProviderValidation()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            When(f => f.TypeProvider == TypeProvider.Physicalperson, () =>
            {
                RuleFor(f => f.Document.Length).Equal(CpfValidacao.TamanhoCpf)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CpfValidacao.Validar(f.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });

            When(f => f.TypeProvider == TypeProvider.JuridicPerson, () =>
            {
                RuleFor(f => f.Document.Length).Equal(CnpjValidacao.TamanhoCnpj)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CnpjValidacao.Validar(f.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }
    }
}
