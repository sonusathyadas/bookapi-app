# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Depending on your project, you may need to adjust this Dockerfile.

# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BookAPI/BookAPI.csproj", "BookAPI/"]
RUN dotnet restore "BookAPI/BookAPI.csproj"
COPY . .
WORKDIR "/src/BookAPI"
RUN dotnet build "BookAPI.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "BookAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookAPI.dll"]