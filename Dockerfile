# development
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS development

RUN mkdir -p /home/dotnet/EST.MIT.Payment.Function.Test/ /home/dotnet/EST.MIT.Payment.Function/

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function.Test/*.csproj ./EST.MIT.Payment.Function.Test/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.DataAccess/*.csproj ./EST.MIT.Payment.DataAccess/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function/*.csproj ./EST.MIT.Payment.Function/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces/*.csproj ./EST.MIT.Payment.Interfaces/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models/*.csproj ./EST.MIT.Payment.Models/
COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services/*.csproj ./EST.MIT.Payment.Services/

RUN dotnet restore ./EST.MIT.Payment.Function.Test/EST.MIT.Payment.Function.Test.csproj
RUN dotnet restore ./EST.MIT.Payment.Function/EST.MIT.Payment.Function.csproj

COPY ./EST.MIT.Payment.Function /src/EST.MIT.Payment.Function
COPY ./EST.MIT.Payment.DataAccess /src/EST.MIT.Payment.DataAccess
COPY ./EST.MIT.Payment.Interfaces /src/EST.MIT.Payment.Interfaces
COPY ./EST.MIT.Payment.Models /src/EST.MIT.Payment.Models
COPY ./EST.MIT.Payment.Services /src/EST.MIT.Payment.Services
RUN cd /src/EST.MIT.Payment.Function && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4.1.3-dotnet-isolated6.0-appservice
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=development ["/home/site/wwwroot", "/home/site/wwwroot"]


# production
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS production

COPY ./EST.MIT.Payment.Function /src/EST.MIT.Payment.Function
COPY ./EST.MIT.Payment.DataAccess /src/EST.MIT.Payment.DataAccess
COPY ./EST.MIT.Payment.Interfaces /src/EST.MIT.Payment.Interfaces
COPY ./EST.MIT.Payment.Models /src/EST.MIT.Payment.Models
COPY ./EST.MIT.Payment.Services /src/EST.MIT.Payment.Services
RUN cd /src/EST.MIT.Payment.Function && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4.1.3-dotnet-isolated6.0-appservice
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=production ["/home/site/wwwroot", "/home/site/wwwroot"]

# ARG PARENT_VERSION=1.5.0-dotnet6.0

# # Development
# FROM defradigital/dotnetcore-development:$PARENT_VERSION AS development

# ARG PARENT_VERSION

# LABEL uk.gov.defra.parent-image=defra-dotnetcore-development:${PARENT_VERSION}

# RUN mkdir -p /home/dotnet/EST.MIT.Payment.DataAccess/ /home/dotnet/EST.MIT.Payment.Function/ /home/dotnet/EST.MIT.Payment.Function.Test/ /home/dotnet/EST.MIT.Payment.Interfaces/ /home/dotnet/EST.MIT.Payment.Models/ /home/dotnet/EST.MIT.Payment.Services/

# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.DataAccess/*.csproj ./EST.MIT.Payment.DataAccess/
# RUN dotnet restore ./EST.MIT.Payment.DataAccess/EST.MIT.Payment.DataAccess.csproj

# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function/*.csproj ./EST.MIT.Payment.Function/
# RUN dotnet restore ./EST.MIT.Payment.Function/EST.MIT.Payment.Function.csproj

# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function.Test/*.csproj ./EST.MIT.Payment.Function.Test/
# RUN dotnet restore ./EST.MIT.Payment.Function.Test/EST.MIT.Payment.Function.Test.csproj

# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces/*.csproj ./EST.MIT.Payment.Interfaces/
# RUN dotnet restore ./EST.MIT.Payment.Interfaces/EST.MIT.Payment.Interfaces.csproj

# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models/*.csproj ./EST.MIT.Payment.Models/
# RUN dotnet restore ./EST.MIT.Payment.Models/EST.MIT.Payment.Models.csproj

# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services/*.csproj ./EST.MIT.Payment.Services/
# RUN dotnet restore ./EST.MIT.Payment.Services/EST.MIT.Payment.Services.csproj


# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.DataAccess/ ./EST.MIT.Payment.DataAccess/
# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function/ ./EST.MIT.Payment.Function/
# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function.Test/ ./EST.MIT.Payment.Function.Test/
# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Interfaces/ ./EST.MIT.Payment.Interfaces/
# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Models/ ./EST.MIT.Payment.Models/
# COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Services/ ./EST.MIT.Payment.Services/


# RUN dotnet publish ./EST.MIT.Payment.Function/ -c Release -o /home/dotnet/out

# ARG PORT=3000
# ENV PORT ${PORT}
# EXPOSE ${PORT}

# CMD dotnet watch --project ./EST.MIT.Payment.Function run --urls "http://*:${PORT}"

# # Production
# FROM defradigital/dotnetcore:$PARENT_VERSION AS production

# ARG PARENT_VERSION
# ARG PARENT_REGISTRY

# LABEL uk.gov.defra.parent-image=defra-dotnetcore-development:${PARENT_VERSION}

# ARG PORT=3000
# ENV ASPNETCORE_URLS=http://*:${PORT}
# EXPOSE ${PORT}

# COPY --from=development /home/dotnet/out/ ./

# CMD dotnet EST.MIT.Payment.Function.dll