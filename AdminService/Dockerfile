# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY src/Services/AdminService/AdminService.csproj Services/AdminService/
COPY src/BuildingBlock/Utils/Utils.csproj BuildingBlock/Utils/
COPY src/BuildingBlock/DataUtils/DataUtils.csproj BuildingBlock/DataUtils/
RUN dotnet restore Services/AdminService/AdminService.csproj

# Copy the full source
COPY . .

# Publish app
WORKDIR /src/Services/AdminService
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "AdminService.dll"]
