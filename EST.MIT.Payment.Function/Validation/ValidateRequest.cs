using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;

namespace EST.MIT.Payment.Function.Validation;

public static class ValidateRequest
{
    const string schemaJson = @"{
        ""$schema"": ""http://json-schema.org/draft-04/schema#"",
        ""type"": ""object"",
        ""properties"": {
            ""schemeType"": {
            ""type"": ""string""
            },
            ""paymentRequestsBatches"": {
            ""type"": ""array"",
            ""items"": [
                {
                ""type"": ""object"",
                ""properties"": {
                    ""id"": {
                    ""type"": ""string""
                    },
                    ""accountType"": {
                    ""type"": ""string""
                    },
                    ""organisation"": {
                    ""type"": ""string""
                    },
                    ""schemeType"": {
                    ""type"": ""string""
                    },
                    ""paymentRequests"": { 
                    ""type"": ""array"",
                    ""items"": [
                        {
                        ""type"": ""object"",
                        ""properties"": {
                            ""paymentRequestId"": {
                            ""type"": ""string""
                            },
                            ""frn"": {
                            ""type"": ""integer""
                            },
                            ""sbi"": {
                            ""type"": ""integer""
                            },
                            ""vendor"": {
                            ""type"": ""string""
                            },
                            ""currency"": {
                            ""type"": ""string""
                            },
                            ""description"": {
                            ""type"": ""string""
                            },
                            ""originalInvoiceNumber"": {
                            ""type"": ""string""
                            },
                            ""originalSettlementDate"": {
                            ""type"": ""string""
                            },
                            ""sourceSystem"": {
                            ""type"": ""string""
                            },
                            ""marketingYear"": {
                            ""type"": ""integer""
                            }, 
                            ""paymentRequestNumber"": {
                            ""type"": ""integer""
                            },
                            ""invoiceCorrectionReference"": {
                            ""type"": ""string""
                            },
                            ""recoveryDate"": {
                            ""type"": ""string""
                            },
                            ""agreementNumber"": {
                            ""type"": ""string""
                            }, 
                            ""value"": {
                            ""type"": [""integer"", ""number""]
                            },
                            ""dueDate"": {
                            ""type"": ""string""
                            },
                            ""invoiceLines"": {
                            ""type"": ""array"",
                            ""items"": [
                                {
                                ""type"": ""object"",
                                ""properties"": {
                                    ""value"": {
                                    ""type"": [""integer"", ""number""]
                                    }, 
                                    ""schemeCode"": {
                                    ""type"": ""string""
                                    },
                                    ""description"": {
                                    ""type"": ""string""
                                    },
                                    ""fundCode"": {
                                    ""type"": ""string""
                                    },
                                    ""mainAccount"": {
                                    ""type"": ""string""
                                    },
                                    ""deliveryBody"": {
                                    ""type"": ""string""
                                    },
                                    ""marketingYear"": {
                                    ""type"": ""integer""
                                    }
                                },
                                ""required"": [
                                    ""value"",                                   
                                    ""schemeCode"",
                                    ""description"",                                   
                                    ""mainAccount"",
                                    ""deliveryBody"",
                                    ""marketingYear"",
                                    ""fundCode""
                                ]
                                }
                            ]
                            }
                            },
                        ""required"": [
                            ""paymentRequestId"",
                            ""frn"",
                            ""sbi"",
                            ""sourceSystem"",
                            ""marketingYear"", 
                            ""paymentRequestNumber"",
                            ""agreementNumber"",
                            ""value"",
                            ""dueDate"",
                            ""invoiceLines"" 
                        ]
                        }
                    ]
                    },
                    ""status"": {
                    ""type"": ""string""
                    },
                    ""reference"": {
                    ""type"": ""string""
                    },
                    ""created"": {
                    ""type"": ""string""
                    },
                    ""updated"": {
                    ""type"": ""string""
                    },
                    ""createdBy"": {
                    ""type"": ""string""
                    },
                    ""updatedBy"": {
                    ""type"": ""string""
                    }
                },
                ""required"": [
                    ""id"", 
                    ""accountType"",
                    ""organisation"",
                    ""schemeType"",
                    ""paymentRequests"",
                    ""status"",
                    ""reference"",
                    ""created"",
                    ""updated"",
                    ""createdBy"",
                    ""updatedBy""
                ]
                }
            ]
            }
        },
        ""required"": [
            ""paymentRequestsBatches""
        ]
        }";

    public static bool IsValid(string importRequest)
    {
        var schema = JSchema.Parse(schemaJson);
        var parseImportRequest = JObject.Parse(importRequest);
        return parseImportRequest.IsValid(schema);
    }

    // Only called if initial validation fails
    public static IList<string> GetValidationErrors(string importRequest)
    {
        var schema = JSchema.Parse(schemaJson);
        var parseImportRequest = JObject.Parse(importRequest);
        IList<string> errorMessages;
        var valid = parseImportRequest.IsValid(schema, out errorMessages);
        return valid ? new List<string>() : errorMessages;
    }
}