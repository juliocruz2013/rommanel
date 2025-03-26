using FluentValidation;
using RommanelTeste.Common;
using RommanelTeste.Domain.Enumerators;

namespace RommanelTeste.Application.Customer.Commands.CustomerCreate;

public class CustomerCreateCommandValidator : AbstractValidator<CustomerCreateCommandRequest>
{
    public CustomerCreateCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull().WithMessage("Nome n�o pode ser nulo.")
            .NotEmpty().WithMessage("Nome n�o pode ser vazio.")
            .WithMessage(TypeError.NameRequired.GetDescription());

        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("Documento n�o pode ser nulo.")
            .NotEmpty().WithMessage("Documento n�o pode ser vazio.")
            .Must(x => Extensions.IsValidDocument(x.UnMask()))
            .WithMessage(TypeError.InvalidDocument.GetDescription());

        RuleFor(x => x.BirthDate)
            .NotNull().WithMessage("Data de Nascimento n�o pode ser nulo.")
            .NotEmpty().WithMessage("Data de Nascimento n�o pode ser vazio.")
            .Must(HaveMinimumAge)
            .When(x => !x.IsCompany).WithMessage(TypeError.MinimumAge.GetDescription());

        RuleFor(x => x.Phone)
            .NotNull().WithMessage("Telefone n�o pode ser nulo.")
            .NotEmpty().WithMessage("Telefone n�o pode ser vazio.")
            .Must(x => x.IsValidPhoneNumber())
            .WithMessage(TypeError.InvalidPhone.GetDescription());

        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email n�o pode ser nulo.")
            .NotEmpty().WithMessage("Email n�o pode ser vazio.")
            .Must(x => x.IsValidEmail())
            .WithMessage(TypeError.InvalidEmail.GetDescription());

        RuleFor(x => x.StateRegistration)
            .NotNull().WithMessage("Inscri��o Estadual n�o pode ser nula.")
            .NotEmpty().WithMessage("Inscri��o Estadual n�o pode ser vazia.")
            .When(x => x.IsCompany && !x.IsExempt.Value).WithMessage(TypeError.StateRegistrationRequired.GetDescription());

        RuleFor(x => x.Address.ZipCode)
           .NotNull().WithMessage("CEP n�o pode ser nulo.")
           .NotEmpty().WithMessage("CEP n�o pode ser vazio.")
           .WithMessage(TypeError.ZipCodeRequired.GetDescription())
           .Matches(@"^\d+$").WithMessage(TypeError.ZipCodeOnlyNumbers.GetDescription());

        RuleFor(x => x.Address.Street)
            .NotNull().WithMessage("Rua n�o pode ser nulo.")
            .NotEmpty().WithMessage("Rua n�o pode ser vazio.")
            .WithMessage(TypeError.StreetRequired.GetDescription());

        RuleFor(x => x.Address.Number)
            .NotNull().WithMessage("N�mero n�o pode ser nulo.")
            .NotEmpty().WithMessage("N�mero n�o pode ser vazio.")
            .WithMessage(TypeError.Required.GetDescription());

        RuleFor(x => x.Address.Neighborhood)
            .NotNull().WithMessage("Bairro n�o pode ser nulo.")
            .NotEmpty().WithMessage("Bairro n�o pode ser vazio.")
            .WithMessage(TypeError.NeighborhoodRequired.GetDescription());

        RuleFor(x => x.Address.City)
            .NotNull().WithMessage("Cidade n�o pode ser nulo.")
            .NotEmpty().WithMessage("Cidade n�o pode ser vazio.")
            .WithMessage(TypeError.CityRequired.GetDescription());

        RuleFor(x => x.Address.State)
            .NotNull().WithMessage("Estado n�o pode ser nula.")
            .NotEmpty().WithMessage("Estado n�o pode ser vazio.")
            .WithMessage(TypeError.StateRequired.GetDescription());
    }

    private bool HaveMinimumAge(DateTime birthDate) => birthDate <= DateTime.Today.AddYears(-18);
}
