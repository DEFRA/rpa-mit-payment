# rpa-mit-payment

This repository contains an azure function with a Service Bus trigger, the messages to the service bus are sent via other services, its use is as a method of sending complete validated invoices to the system that can produce a payment.

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=est-mit-payment&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=est-mit-payment) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=est-mit-payment&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=est-mit-payment) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=est-mit-payment&metric=coverage)](https://sonarcloud.io/summary/new_code?id=est-mit-payment) [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=est-mit-payment&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=est-mit-payment)

## Requirements

Amend as needed for your distribution, this assumes you are using windows with WSL. 

- <details>
    <summary> .NET 8 SDK </summary>
    
    #### Basic instructions for installing the .NET 8 SDK on a debian based system.
  
    Amend as needed for your distribution.

    ```bash
    wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
    ```
</details>

- <details>
    <summary> Azure Functions Core Tools </summary>
    
    ```bash
    sudo apt-get install azure-functions-core-tools-4
    ```
</details>

- [Docker](https://docs.docker.com/desktop/install/linux-install/)
- Service Bus Queue

---

## Local Setup

To run this service locally complete the following steps.
### Create Local Settings

Create a local.setttings.json file with the following content.

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
    }
}
```

### Set up user secrets

Use the secrets-template to create a "secrets.json" in the same folder location.

The schemes key should be a comma separated list

```json
{
	"Schemes": "Scheme1,Scheme2"
}
```

Once this is done run the following command to add the projects user secrets

```bash
cat secrets.json | dotnet user-secrets set
```

These values can also be added to the local settings file, but the preferred method is via user secrets.
### Startup

To start the function locally.

```bash
func start
```

If running multiple function apps locally you might encounter a port conflict error as they all try to run on port 7071. If this happens use a command such as this entering a port that is free.

```bash
func start --port 7072
```

---
## Running in Docker

To create the application as a docker container run the following command in the parent directory.

```bash
docker compose up
```

---

## Usage / Endpoints

### Payment Creation
> Function Trigger: ServiceBusTrigger
> #### Endpoint
> Uses the Service Bus queue trigger named from the environment variable `%PaymentQueueName%`
> #### Action
> Receives payment requests and processes them. If the request is valid, it sends it forward using the service bus after performing some validations. Errors during processing are logged, and messages are sent to an event queue service indicating the status of the payment processing.