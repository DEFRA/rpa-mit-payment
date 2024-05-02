# development
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development

RUN mkdir -p /home/dotnet/EST.MIT.Payment.Function.Tests/ /home/dotnet/EST.MIT.Payment.Function/

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function.Tests/*.csproj ./EST.MIT.Payment.Function.Tests/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function/*.csproj ./EST.MIT.Payment.Function/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces/*.csproj ./EST.MIT.Payment.Interfaces/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models/*.csproj ./EST.MIT.Payment.Models/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services/*.csproj ./EST.MIT.Payment.Services/

RUN dotnet restore ./EST.MIT.Payment.Function.Tests/EST.MIT.Payment.Function.Tests.csproj
RUN dotnet restore ./EST.MIT.Payment.Function/EST.MIT.Payment.Function.csproj

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function /src/EST.MIT.Payment.Function
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces /src/EST.MIT.Payment.Interfaces
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models /src/EST.MIT.Payment.Models
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services /src/EST.MIT.Payment.Services

COPY ./RPA.MIT.Notification.Function /src
RUN cd /src && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=development ["/home/site/wwwroot", "/home/site/wwwroot"]

# production
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS production

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function /src/EST.MIT.Payment.Function
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces /src/EST.MIT.Payment.Interfaces
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models /src/EST.MIT.Payment.Models
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services /src/EST.MIT.Payment.Services

RUN cd /src/EST.MIT.Payment.Function && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj -c Release --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=production ["/home/site/wwwroot", "/home/site/wwwroot"]

