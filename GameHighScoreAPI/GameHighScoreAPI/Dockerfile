#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /src
COPY ["GameHighScoreAPI.csproj", "GameHighScoreAPI/"]
RUN dotnet restore "GameHighScoreAPI/GameHighScoreAPI.csproj"
COPY . "GameHighScoreAPI"
WORKDIR "/src/GameHighScoreAPI"
RUN dotnet build "GameHighScoreAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameHighScoreAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameHighScoreAPI.dll"]