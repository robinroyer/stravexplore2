FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StravaConsumer.API.csproj", "./"]
RUN dotnet restore "StravaConsumer.API.csproj"
COPY . .
RUN dotnet build "StravaConsumer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StravaConsumer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StravaConsumer.API.dll"]
