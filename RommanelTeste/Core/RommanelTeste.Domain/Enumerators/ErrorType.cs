using System.ComponentModel;

namespace RommanelTeste.Domain.Enumerators;

public enum TypeError
{
    [Description("Ocorreu um erro interno.")]
    DefaultError = 1,

    [Description("Campo obrigat�rio.")]
    Required = 2,

    [Description("CPF ou CNPJ inv�lido.")]
    InvalidDocument = 3,

    [Description("Senhas devem conter ao menos um caracter especial.")]
    PasswordRequiresNonAlphanumeric = 4,

    [Description("Email j� est� sendo utilizado.")]
    DuplicateEmail = 5,

    [Description("A permiss�o j� est� sendo utilizada.")]
    DuplicateRoleName = 6,

    [Description("Login j� est� sendo utilizado.")]
    DuplicateUserName = 7,

    [Description("Email inv�lido.")]
    InvalidEmail = 8,

    [Description("A permiss�o � inv�lida.")]
    InvalidRoleName = 9,

    [Description("Token inv�lido.")]
    InvalidToken = 10,

    [Description("Login � inv�lido, deve conter apenas letras ou d�gitos.")]
    InvalidUserName = 11,

    [Description("J� existe um usu�rio com este login.")]
    LoginAlreadyAssociated = 12,

    [Description("Senha incorreta.")]
    PasswordMismatch = 13,

    [Description("Senhas devem conter ao menos um digito ('0'-'9').")]
    PasswordRequiresDigit = 14,

    [Description("Senhas devem conter ao menos um caracter com letra min�scula ('a'-'z').")]
    PasswordRequiresLower = 15,

    [Description("Senhas devem conter ao menos um caracter com letra mai�scula ('A'-'Z').")]
    PasswordRequiresUpper = 16,

    [Description("Senhas devem conter ao menos 8 caracteres.")]
    PasswordTooShort = 17,

    [Description("Usu�rio n�o encontrado.")]
    UserNotFound = 18,

    [Description("Usu�rio n�o autorizado.")]
    Unauthorized = 19,

    [Description("Usu�rio � obrigat�rio.")]
    UserRequired = 20,

    [Description("Email � obrigat�rio.")]
    EmailRequired = 21,

    [Description("Senha � obrigat�rio.")]
    PasswordRequired = 22,

    [Description("Confirma��o de Senha � obrigat�rio.")]
    ConfirmPasswordRequired = 23,

    [Description("N�mero de telefone inv�lido")]
    InvalidPhone = 24,

    [Description("As senhas devem ser iguais.")]
    ConfirmPasswordNotEqual = 25,

    [Description("Senha incorreta.")]
    IncorrectPassword = 26,

    [Description("Id do usu�rio n�o encontrado.")]
    InvalidId = 27,

    [Description("Permiss�o de usu�rio n�o encontrada.")]
    RoleNotFound = 28,

    [Description("Cliente n�o encontrado.")]
    CustomerNotFound = 29,

    [Description("Cliente j� possui endere�o cadastrado.")]
    CustomerHasAddress = 30,

    [Description("J� existe um cliente cadastrado com o mesmo E-mail ou N�mero de Documento, verifique os dados e tente novamente.")]
    CustomerRegisteredWithTheSameData = 31,

    [Description("A idade m�nima para pessoa f�sica � de 18 anos.")]
    MinimumAge = 32,

    [Description("Inscri��o Estadual � obrigat�ria quando a empresa n�o for isenta.")]
    StateRegistrationRequired = 33,

    [Description("Nome � obrigat�rio.")]
    NameRequired = 34,

    [Description("CEP � obrigat�rio.")]
    ZipCodeRequired = 35,

    [Description("O CEP deve conter apenas n�meros.")]
    ZipCodeOnlyNumbers = 36,

    [Description("Rua � obrigat�rio.")]
    StreetRequired = 37,
    
    [Description("N�mero � obrigat�rio.")]
    NumberRequired = 38,

    [Description("Bairro � obrigat�rio.")]
    NeighborhoodRequired = 39,

    [Description("Cidade � obrigat�rio.")]
    CityRequired = 40,

    [Description("Estado � obrigat�rio.")]
    StateRequired = 41
}
