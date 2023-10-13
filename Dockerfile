# development
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS development

RUN mkdir -p /home/dotnet/EST.MIT.Payment.Function.Tests/ /home/dotnet/EST.MIT.Payment.Function/

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function.Tests/*.csproj ./EST.MIT.Payment.Function.Tests/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.DataAccess/*.csproj ./EST.MIT.Payment.DataAccess/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function/*.csproj ./EST.MIT.Payment.Function/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces/*.csproj ./EST.MIT.Payment.Interfaces/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models/*.csproj ./EST.MIT.Payment.Models/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services/*.csproj ./EST.MIT.Payment.Services/

RUN dotnet restore ./EST.MIT.Payment.Function.Tests/EST.MIT.Payment.Function.Tests.csproj
RUN dotnet restore ./EST.MIT.Payment.Function/EST.MIT.Payment.Function.csproj

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function /src/EST.MIT.Payment.Function
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.DataAccess /src/EST.MIT.Payment.DataAccess
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces /src/EST.MIT.Payment.Interfaces
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models /src/EST.MIT.Payment.Models
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services /src/EST.MIT.Payment.Services
RUN cd /src/EST.MIT.Payment.Function && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4.1.3-dotnet-isolated6.0-appservice
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

ENV ASPNETCORE_URLS=http://+:3000
ENV FUNCTIONS_WORKER_RUNTIME=dotnet-isolated

COPY --from=development ["/home/site/wwwroot", "/home/site/wwwroot"]


# production
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS production

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function /src/EST.MIT.Payment.Function
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.DataAccess /src/EST.MIT.Payment.DataAccess
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces /src/EST.MIT.Payment.Interfaces
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models /src/EST.MIT.Payment.Models
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services /src/EST.MIT.Payment.Services
RUN cd /src/EST.MIT.Payment.Function && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj -c Release --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4.1.3-dotnet-isolated6.0-appservice
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

ENV ASPNETCORE_URLS=http://+:3000
ENV FUNCTIONS_WORKER_RUNTIME=dotnet-isolated

COPY --from=production ["/home/site/wwwroot", "/home/site/wwwroot"]
