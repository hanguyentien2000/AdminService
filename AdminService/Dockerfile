# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS build
WORKDIR /src
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AdminService.csproj", "AdminService/"]

RUN dotnet restore "AdminService/AdminService.csproj"
COPY . .
WORKDIR "/src/AdminService"
RUN dotnet build "AdminService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminService.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminService.dll"]