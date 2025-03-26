using System.ComponentModel;

namespace RommanelTeste.Domain.Enumerators;

public enum TypeError
{
    [Description("Ocorreu um erro interno.")]
    DefaultError = 1,

    [Description("Campo obrigatório.")]
    Required = 2,

    [Description("CPF ou CNPJ inválido.")]
    InvalidDocument = 3,

    [Description("Senhas devem conter ao menos um caracter especial.")]
    PasswordRequiresNonAlphanumeric = 4,

    [Description("Email já está sendo utilizado.")]
    DuplicateEmail = 5,

    [Description("A permissão já está sendo utilizada.")]
    DuplicateRoleName = 6,

    [Description("Login já está sendo utilizado.")]
    DuplicateUserName = 7,

    [Description("Email inválido.")]
    InvalidEmail = 8,

    [Description("A permissão é inválida.")]
    InvalidRoleName = 9,

    [Description("Token inválido.")]
    InvalidToken = 10,

    [Description("Login é inválido, deve conter apenas letras ou dígitos.")]
    InvalidUserName = 11,

    [Description("Já existe um usuário com este login.")]
    LoginAlreadyAssociated = 12,

    [Description("Senha incorreta.")]
    PasswordMismatch = 13,

    [Description("Senhas devem conter ao menos um digito ('0'-'9').")]
    PasswordRequiresDigit = 14,

    [Description("Senhas devem conter ao menos um caracter com letra minúscula ('a'-'z').")]
    PasswordRequiresLower = 15,

    [Description("Senhas devem conter ao menos um caracter com letra maiúscula ('A'-'Z').")]
    PasswordRequiresUpper = 16,

    [Description("Senhas devem conter ao menos 8 caracteres.")]
    PasswordTooShort = 17,

    [Description("Usuário não encontrado.")]
    UserNotFound = 18,

    [Description("Usuário não autorizado.")]
    Unauthorized = 19,

    [Description("Usuário é obrigatório.")]
    UserRequired = 20,

    [Description("Email é obrigatório.")]
    EmailRequired = 21,

    [Description("Senha é obrigatório.")]
    PasswordRequired = 22,

    [Description("Confirmação de Senha é obrigatório.")]
    ConfirmPasswordRequired = 23,

    [Description("Número de telefone inválido")]
    InvalidPhone = 24,

    [Description("As senhas devem ser iguais.")]
    ConfirmPasswordNotEqual = 25,

    [Description("Senha incorreta.")]
    IncorrectPassword = 26,

    [Description("Id do usuário não encontrado.")]
    InvalidId = 27,

    [Description("Permissão de usuário não encontrada.")]
    RoleNotFound = 28,

    [Description("Cliente não encontrado.")]
    CustomerNotFound = 29,

    [Description("Cliente já possui endereço cadastrado.")]
    CustomerHasAddress = 30,

    [Description("Já existe um cliente cadastrado com o mesmo E-mail ou Número de Documento, verifique os dados e tente novamente.")]
    CustomerRegisteredWithTheSameData = 31,

    [Description("A idade mínima para pessoa física é de 18 anos.")]
    MinimumAge = 32,

    [Description("Inscrição Estadual é obrigatória quando a empresa não for isenta.")]
    StateRegistrationRequired = 33,

    [Description("Nome é obrigatório.")]
    NameRequired = 34,

    [Description("CEP é obrigatório.")]
    ZipCodeRequired = 35,

    [Description("O CEP deve conter apenas números.")]
    ZipCodeOnlyNumbers = 36,

    [Description("Rua é obrigatório.")]
    StreetRequired = 37,
    
    [Description("Número é obrigatório.")]
    NumberRequired = 38,

    [Description("Bairro é obrigatório.")]
    NeighborhoodRequired = 39,

    [Description("Cidade é obrigatório.")]
    CityRequired = 40,

    [Description("Estado é obrigatório.")]
    StateRequired = 41
}
