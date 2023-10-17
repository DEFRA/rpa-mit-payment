# Introduction 
This repository contains the code for consuming the payment generator for MIT
The Invoices created in MIT need to proxied to SPS via Service Bus. Any processed messages are audited by the rpa-mit-events service (by sending a message to the rpa-mit-events queue).
 
# Getting Started
## Azurite

Follow the following guide to setup Azurite:

- [Azurite emulator for local Azure Storage development](https://dev.azure.com/defragovuk/DEFRA-EST/_wiki/wikis/DEFRA-EST/7722/Azurite-emulator-for-local-Azure-Storage-development)

## Storage

The function app uses Azure Storage for Queue but you will need to set up an appropriate Service Bus queue in Azure.

The function app requires:

- Storage Queue name: `rpa-mit-payment`
- Storage Queue name: `rpa-mit-events`
- Service Bus Queue name: `rpa-mit-payment`

## local.settings
```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "QueueConnectionString": "UseDevelopmentStorage=true",
        "EventQueueName": "rpa-mit-events",
        "PaymentQueueName": "rpa-mit-payment",
        "ServiceBusConnectionString": "--SECRET--",
        "ServiceBusQueueName": "rpa-mit-payment",
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

- [Use dependency injection in .NET Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
