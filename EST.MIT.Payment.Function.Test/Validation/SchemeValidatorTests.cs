using EST.MIT.Payment.Services;
using EST.MIT.Payment.Interfaces;

namespace EST.MIT.Payment.Function.Test.Validation;

public class SchemeValidatorTests
{
    private readonly ISchemeValidator _schemeValidator;

    public SchemeValidatorTests()
    {
        Environment.SetEnvironmentVariable("Schemes", "scheme1,scheme2,scheme3");
        _schemeValidator = new SchemeValidator();
    }

    [Theory]
    [InlineData("scheme1", true, "scheme1,scheme2,scheme3")]
    [InlineData("scheme4", false, "scheme1,scheme2,scheme3")]
    [InlineData("scheme1", false, null)]
    [InlineData("scheme1", false, "")]
    public void ValueExists_Returns_True_or_False(string scheme, bool expectedResult, string envVariableValue)
    {
        Environment.SetEnvironmentVariable("Schemes", envVariableValue);
        var schemeValidator = new SchemeValidator();
        var result = schemeValidator.ValueExists(scheme);

        Assert.Equal(expectedResult, result);
    }
}
