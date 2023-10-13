using EST.MIT.Payment.Function.Validation;

namespace EST.MIT.Payment.Function.Tests.Validation;

public class ValidateRequestTests
{
    [Fact]
    public void IsValid_ReturnsTrue_WhenJsonIsValid()
    {
        var validJson = @"
            {
                ""schemeType"": ""TestScheme"",
                ""paymentRequestsBatches"": [
                    {
                        ""id"": ""1"",
                        ""invoiceType"": ""TestType"",
                        ""accountType"": ""TestAccount"",
                        ""organisation"": ""TestOrganisation"",
                        ""schemeType"": ""TestSchemeType"",
                        ""paymentRequests"": [],
                        ""status"": ""TestStatus"",
                        ""reference"": ""TestReference"",
                        ""created"": ""2023-05-12"",
                        ""updated"": ""2023-05-12"",
                        ""createdBy"": ""TestCreator"",
                        ""updatedBy"": ""TestUpdater""
                    }
                ]
            }";

        var result = ValidateRequest.IsValid(validJson);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenJsonIsInvalid()
    {
        var invalidJson = "{}";

        var result = ValidateRequest.IsValid(invalidJson);

        Assert.False(result);
    }
}
