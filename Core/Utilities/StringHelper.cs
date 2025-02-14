using Core.Entities;
using Core.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace Core.Utilities;

public static class StringHelper
{
    public static string Hash(this string inputString)
        => BCrypt.Net.BCrypt.HashPassword(inputString);

    public static bool Verify(string pass, string oldPass)
        => BCrypt.Net.BCrypt.Verify(pass, oldPass);

    private static readonly Random Random = new();

    public static int GenerateRandom(int min, int max) => Random.Next(min, max);

    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return input.Substring(0, 1).ToUpper() + input.Substring(1);
    }

    public static ServiceType GetServiceTypeFromPodName(this string podName)
    {
        // Obtenha o valor após o primeiro traço e capitalize a primeira letra
        string serviceTypeStr = CapitalizeFirstLetter(podName.Split("-")[1]);

        // Tenta converter a string para o enum ServiceType
        if (Enum.TryParse(serviceTypeStr, out ServiceType serviceType))
        {
            return serviceType;
        }

        // Se não for possível converter, você pode retornar um valor padrão ou lançar uma exceção
        throw new ArgumentException($"Invalid service type: {serviceTypeStr}");
    }

    public static string GenerateRandomStringFromGuid(int length = 32)
    {
        string guidString = Guid.NewGuid().ToString("N").Replace("-", "");
        return guidString.Substring(0, length);
    }
    public static string GetIdFromGuidString(this string guid)
    {
        string guidString = Guid.Parse(guid).ToString("N");
        return guidString.Substring(0, 8);
    }

    public static double GetAvailableStorageSizeInBytes(string storageSizeStr, ICollection<Service> services)
    {
        if (storageSizeStr is null) return 0;

        var storageSizeInt = GetBytesFromSize(storageSizeStr);

        if (!services.Any()) return storageSizeInt;

        var sum = services.Sum(x => GetBytesFromSize(x.StorageSize));

        return storageSizeInt - sum;
    }

    public static double GetBytesFromSize(string sizeStr)
    {
        var units = new Dictionary<string, double>
        {
            { "Mi", Math.Pow(1024, 2) },
            { "Gi", Math.Pow(1024, 3) },
            { "m", 0.001 }
        };

        Match number = Regex.Match(sizeStr, @"\d+(\.\d+)?");
        Match unit = Regex.Match(sizeStr, @"[a-zA-Z]+");

        if (!number.Success || !unit.Success || !units.ContainsKey(unit.Value))
        {
            throw new ArgumentException("Unidade não reconhecida.");
        }

        double value = double.Parse(number.Value);

        return value * units[unit.Value];
    }

}
