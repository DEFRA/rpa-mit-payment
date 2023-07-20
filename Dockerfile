# development
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS development

RUN mkdir -p /home/dotnet/EST.MIT.Payment.Function.Test/ /home/dotnet/EST.MIT.Payment.Function/

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function.Test/*.csproj ./EST.MIT.Payment.Function.Test/
RUN dotnet restore ./EST.MIT.Payment.Function.Test/EST.MIT.Payment.Function.Test.csproj

COPY --chown=dotnet:dotnet ./EST.MIT.Payment.Function/*.csproj ./EST.MIT.Payment.Function/
RUN dotnet restore ./EST.MIT.Payment.Function/EST.MIT.Payment.Function.csproj

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