# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Nonfy.Api/Nonfy.Api.csproj", "Nonfy.Api/"]
COPY ["Nonfy.Domain/Nonfy.Domain.csproj", "Nonfy.Domain/"]
COPY ["Nonfy.Application/Nonfy.Application.csproj", "Nonfy.Application/"]
COPY ["Nonfy.Infrastructure/Nonfy.Infrastructure.csproj", "Nonfy.Infrastructure/"]
RUN dotnet restore "Nonfy.Api/Nonfy.Api.csproj"

# Install EF tool
RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy everything else and build
COPY . .
WORKDIR "/src/Nonfy.Api"
RUN dotnet build "Nonfy.Api.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Nonfy.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy source for EF migrations
COPY . /src
# Install EF tool in the final stage
RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "Nonfy.Api.dll"]