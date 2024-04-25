using System;
using System.Linq;
using EST.MIT.Payment.Interfaces;

namespace EST.MIT.Payment.Function.Validation;

public class SchemeValidator : ISchemeValidator
{
    private readonly string[] _schemes;

    public SchemeValidator()
    {
        var schemes = Environment.GetEnvironmentVariable("Schemes");
        _schemes = schemes != null ? schemes.Split(",") : Array.Empty<string>();
    }

    public bool ValueExists(string value)
    {
        return _schemes.Contains(value);
    }
}
