using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace RommanelTeste.Common;

public static class Extensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attributes = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes?.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : value.ToString() ?? value.ToString();
    }

    public static bool HasValue(this string value)
    {
        return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
    }

    public static string ToJson(this object obj, bool igonoreNull = false)
    {
        if (igonoreNull)
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Local });

        return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, DateTimeZoneHandling = DateTimeZoneHandling.Local });
    }

    public static T FromJson<T>(this string value)
    {
        return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
    }

    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
    }

    public static string UnMask(this string value)
    {
        return value.HasValue() ? value.Replace("-", "").Replace(".", "").Replace("/", "") : value;
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var regex = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        return regex.IsMatch(email);
    }

    public static bool IsValidDocument(string document)
    {
        document = Sanitize(document); //Retorna apenas digitos

        if (document.Length == 11)
            return ValidateCpf(document);
        else if (document.Length == 14)
            return ValidateCnpj(document);

        return false;
    }

    public static string Sanitize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return Regex.Replace(input, @"[^\d]", string.Empty);
    }

    private static bool ValidateCpf(string cpf)
    {
        if (!Regex.IsMatch(cpf, @"^\d{11}$"))
            return false;

        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith(digito1.ToString() + digito2.ToString());
    }

    private static bool ValidateCnpj(string cnpj)
    {
        if (!Regex.IsMatch(cnpj, @"^\d{14}$"))
            return false;

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        tempCnpj += digito1;
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
    }

    public static bool IsValidPhoneNumber(this string phoneNumber)
    {
        if (phoneNumber == null)
            return false;

        phoneNumber = phoneNumber.UnMask();

        if (phoneNumber.Length != 10 && phoneNumber.Length != 11)
            return false;

        if (!Int64.TryParse(phoneNumber, out long valido))
            return false;

        switch (phoneNumber)
        {
            case "00000000000":
            case "0000000000":
                return false;
            case "11111111111":
            case "1111111111":
                return false;
            case "22222222222":
            case "2222222222":
                return false;
            case "33333333333":
            case "3333333333":
                return false;
            case "44444444444":
            case "4444444444":
                return false;
            case "55555555555":
            case "5555555555":
                return false;
            case "66666666666":
            case "6666666666":
                return false;
            case "77777777777":
            case "7777777777":
                return false;
            case "88888888888":
            case "8888888888":
                return false;
            case "99999999999":
            case "9999999999":
                return false;
        }

        phoneNumber = phoneNumber.ApplyPhoneNumberMask();

        if (phoneNumber.Length != 14 && phoneNumber.Length != 15)
            return false;

        return true;
    }

    public static string ApplyPhoneNumberMask(this string value)
    {
        if (value != null)
        {
            if (value.Length == 11)
            {
                var a = string.Concat(value[0], value[1]);
                var b = string.Concat(value[2], value[3], value[4], value[5], value[6]);
                var c = string.Concat(value[7], value[8], value[9], value[10]);

                return $"({a}) {b}-{c}";
            }
            else if (value.Length == 10)
            {
                var a = string.Concat(value[0], value[1]);
                var b = string.Concat(value[2], value[3], value[4], value[5]);
                var c = string.Concat(value[6], value[7], value[8], value[9]);

                return $"({a}) {b}-{c}";
            }
            else
            {
                return value;
            }
        }
        return "";
    }

    public static T[] GetEnumValues<T>() where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("Ocorreu um erro interno.");

        return (T[])Enum.GetValues(typeof(T));
    }
}
