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
            .NotNull().WithMessage("Nome não pode ser nulo.")
            .NotEmpty().WithMessage("Nome não pode ser vazio.")
            .WithMessage(TypeError.NameRequired.GetDescription());

        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("Documento não pode ser nulo.")
            .NotEmpty().WithMessage("Documento não pode ser vazio.")
            .Must(x => Extensions.IsValidDocument(x.UnMask()))
            .WithMessage(TypeError.InvalidDocument.GetDescription());

        RuleFor(x => x.BirthDate)
            .NotNull().WithMessage("Data de Nascimento não pode ser nulo.")
            .NotEmpty().WithMessage("Data de Nascimento não pode ser vazio.")
            .Must(HaveMinimumAge)
            .When(x => !x.IsCompany).WithMessage(TypeError.MinimumAge.GetDescription());

        RuleFor(x => x.Phone)
            .NotNull().WithMessage("Telefone não pode ser nulo.")
            .NotEmpty().WithMessage("Telefone não pode ser vazio.")
            .Must(x => x.IsValidPhoneNumber())
            .WithMessage(TypeError.InvalidPhone.GetDescription());

        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email não pode ser nulo.")
            .NotEmpty().WithMessage("Email não pode ser vazio.")
            .Must(x => x.IsValidEmail())
            .WithMessage(TypeError.InvalidEmail.GetDescription());

        RuleFor(x => x.StateRegistration)
            .NotNull().WithMessage("Inscrição Estadual não pode ser nula.")
            .NotEmpty().WithMessage("Inscrição Estadual não pode ser vazia.")
            .When(x => x.IsCompany && !x.IsExempt.Value).WithMessage(TypeError.StateRegistrationRequired.GetDescription());

        RuleFor(x => x.Address.ZipCode)
           .NotNull().WithMessage("CEP não pode ser nulo.")
           .NotEmpty().WithMessage("CEP não pode ser vazio.")
           .WithMessage(TypeError.ZipCodeRequired.GetDescription())
           .Matches(@"^\d+$").WithMessage(TypeError.ZipCodeOnlyNumbers.GetDescription());

        RuleFor(x => x.Address.Street)
            .NotNull().WithMessage("Rua não pode ser nulo.")
            .NotEmpty().WithMessage("Rua não pode ser vazio.")
            .WithMessage(TypeError.StreetRequired.GetDescription());

        RuleFor(x => x.Address.Number)
            .NotNull().WithMessage("Número não pode ser nulo.")
            .NotEmpty().WithMessage("Número não pode ser vazio.")
            .WithMessage(TypeError.Required.GetDescription());

        RuleFor(x => x.Address.Neighborhood)
            .NotNull().WithMessage("Bairro não pode ser nulo.")
            .NotEmpty().WithMessage("Bairro não pode ser vazio.")
            .WithMessage(TypeError.NeighborhoodRequired.GetDescription());

        RuleFor(x => x.Address.City)
            .NotNull().WithMessage("Cidade não pode ser nulo.")
            .NotEmpty().WithMessage("Cidade não pode ser vazio.")
            .WithMessage(TypeError.CityRequired.GetDescription());

        RuleFor(x => x.Address.State)
            .NotNull().WithMessage("Estado não pode ser nula.")
            .NotEmpty().WithMessage("Estado não pode ser vazio.")
            .WithMessage(TypeError.StateRequired.GetDescription());
    }

    private bool HaveMinimumAge(DateTime birthDate) => birthDate <= DateTime.Today.AddYears(-18);
}
