# Use the official .NET image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AuthService/AuthService.csproj", "AuthService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "AuthService/AuthService.csproj"
COPY . .
WORKDIR "/src/AuthService"
RUN dotnet build "AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthService.csproj" -c Release -o /app/publish

# Final stage: use the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]