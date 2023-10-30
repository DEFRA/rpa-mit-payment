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
  "schemeType": "AD",
  "paymentRequestsBatches": [
    {
      "id": "1",
      "accountType": "AD",
      "organisation": "FGH",
      "schemeType": "AD",
      "paymentType": "AP",
      "paymentRequests": [
        {
          "paymentRequestId": "2",
          "frn": 56789043,
          "sbi": 4567,
          "vendor": "A",
          "sourceSystem": "sourceSystem",
          "marketingYear": 2023,
          "currency": "GBP",
          "description": "Description",
          "originalInvoiceNumber": "23ER56",
          "originalSettlementDate": "2023-10-19T13:11:17.58488+01:00",
          "recoveryDate": "2023-10-19T13:11:17.584896+01:00",
          "invoiceCorrectionReference": "ERQ567",
          "paymentRequestNumber": 34567,
          "agreementNumber": "12345",
          "value": 2,
          "dueDate": "string",
          "invoiceLines": [
            {
              "value": 3,
              "schemeCode": "D4ERT",
              "description": "This is a description",
              "fundCode": "2ADC",
              "mainAccount": "AccountA",
              "marketingYear": 2022,
              "deliveryBody": "DeliveryBody"
            }
          ]
        }
      ],
      "status": "A",
      "reference": "123",
      "created": "2022-11-05T00:00:00",
      "updated": "2023-10-19T13:11:17.584978+01:00",
      "createdBy": "username1",
      "updatedBy": "username1"
    }
  ]
}


# Build and Test

The easiest way to build the project is with VS2022. It should download all required nuget dependencies.

Run the tests using the VS2022 Test Explorer.

## Useful links

- [Use dependency injection in .NET Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
