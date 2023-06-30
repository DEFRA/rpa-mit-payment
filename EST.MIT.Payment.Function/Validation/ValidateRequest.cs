using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

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
            ""invoices"": {
            ""type"": ""array"",
            ""items"": [
                {
                ""type"": ""object"",
                ""properties"": {
                    ""id"": {
                    ""type"": ""string""
                    },
                    ""invoiceType"": {
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
                            ""sourceSystem"": {
                            ""type"": ""string""
                            },
                            ""marketingYear"": {
                            ""type"": ""integer""
                            },
                            ""deliveryBody"": {
                            ""type"": ""string""
                            },
                            ""paymentRequestNumber"": {
                            ""type"": ""integer""
                            },
                            ""agreementNumber"": {
                            ""type"": ""string""
                            },
                            ""contractNumber"": {
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
                                    ""currency"": {
                                    ""type"": ""string""
                                    },
                                    ""schemeCode"": {
                                    ""type"": ""string""
                                    },
                                    ""description"": {
                                    ""type"": ""string""
                                    },
                                    ""fundCode"": {
                                    ""type"": ""string""
                                    }
                                },
                                ""required"": [
                                    ""value"",
                                    ""currency"",
                                    ""schemeCode"",
                                    ""description"",
                                    ""fundCode""
                                ]
                                }
                            ]
                            },
                            ""appendixReferences"": {
                            ""type"": ""object"",
                            ""properties"": {
                                ""claimReferenceNumber"": {
                                ""type"": ""string""
                                }
                            },
                            ""required"": [
                                ""claimReferenceNumber""
                            ]
                            }
                        },
                        ""required"": [
                            ""paymentRequestId"",
                            ""frn"",
                            ""sourceSystem"",
                            ""marketingYear"",
                            ""deliveryBody"",
                            ""paymentRequestNumber"",
                            ""agreementNumber"",
                            ""contractNumber"",
                            ""value"",
                            ""dueDate"",
                            ""invoiceLines"",
                            ""appendixReferences""
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
                    ""invoiceType"",
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
            ""invoices""
        ]
        }";

    public static bool IsValid(string importRequest)
    {
        var schema = JSchema.Parse(schemaJson);
        var parseImportRequest = JObject.Parse(importRequest);
        return parseImportRequest.IsValid(schema);
    }
}