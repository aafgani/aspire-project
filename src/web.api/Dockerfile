FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5143

ENV ASPNETCORE_URLS=http://+:5143

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/web.api/web.api.csproj", "src/web.api/"]
RUN dotnet restore "src/web.api/web.api.csproj"
COPY . .
WORKDIR "/src/src/web.api"
RUN dotnet build "web.api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "web.api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web.api.dll"]
