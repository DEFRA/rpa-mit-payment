# Introduction 
This repository contains the code for consuming the payment generator for MIT
The Invoices created in MIT need to reach one of two destinations, Strategics Payments (SPS) or Transformation Layer (TL). The destination is determined by the scheme type. Depending of scheme type the Payment Generator will create either a Json message to be place on a Service Bus queue for SPS or an XML file to be placed in Blob Storage for the TL.
 
# Getting Started
## Azurite

Follow the following guide to setup Azurite:

- [Azurite emulator for local Azure Storage development](https://dev.azure.com/defragovuk/DEFRA-EST/_wiki/wikis/DEFRA-EST/7722/Azurite-emulator-for-local-Azure-Storage-development)

## Storage

The function app uses Azure Storage for Table and Queue.

The function app requires:

- Queue name: `Payment`
- Table name: `Payment`

## local.settings
```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "QueueConnectionString": "UseDevelopmentStorage=true",
        "TableConnectionString": "UseDevelopmentStorage=true",
        "BlobConnectionString": "UseDevelopmentStorage=true",
        "PaymentBlobContainer": "Payment",
        "Schemes": "bps,cs"
    }
}
```
## Queue

### Message Example
{
"invoices": [
   {
     "id": "string",
     "invoiceType": "string",
     "accountType": "string",
     "organisation": "string",
     "schemeType": "string",
     "paymentRequests": [
       {
         "paymentRequestId": "string",
         "frn": 0,
         "sourceSystem": "string",
         "marketingYear": 0,
         "deliveryBody": "string",
         "paymentRequestNumber": 0,
         "agreementNumber": "string",
         "contractNumber": "string",
         "value": 0,
         "dueDate": "string",
         "invoiceLines": [
           {
             "value": 0,
             "currency": "string",
             "schemeCode": "string",
             "description": "string",
             "fundCode": "string"
           }
         ],
         "appendixReferences": {
           "claimReferenceNumber": "string"
         }
       }
     ],
     "status": "string",
     "reference": "string",
     "created": "2023-04-03T07:18:19.457Z",
     "updated": "2023-04-03T07:18:19.457Z",
     "createdBy": "string",
     "updatedBy": "string"
   }
]
}

# Build and Test

The easiest way to build the project is with VS2022. It should download all required nuget dependencies.

Run the tests using the VS2022 Test Explorer.

## Useful links

- [gov Notify](https://www.notifications.service.gov.uk/using-notify/api-documentation)

- [Use dependency injection in .NET Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
