using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace Server.Database.Converters;

public sealed class UlongToStringConverter : ValueConverter<ulong, string>
{
    public static readonly UlongToStringConverter Instance = new();

    public UlongToStringConverter()
        : base(
            v => v.ToString(CultureInfo.InvariantCulture),
            v => ulong.Parse(v, CultureInfo.InvariantCulture))
    {
    }
}

