FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ProviderService.Api.Rest/ProviderService.Api.Rest.csproj", "src/ProviderService.Api.Rest/"]
RUN dotnet restore "src/ProviderService.Api.Rest/ProviderService.Api.Rest.csproj"
COPY . .
WORKDIR "/src/src/ProviderService.Api.Rest"
RUN dotnet build "ProviderService.Api.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProviderService.Api.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProviderService.Api.Rest.dll"]
