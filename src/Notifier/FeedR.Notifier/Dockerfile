#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Notifier/FeedR.Notifier/FeedR.Notifier.csproj", "src/Notifier/FeedR.Notifier/"]
RUN dotnet restore "src/Notifier/FeedR.Notifier/FeedR.Notifier.csproj"
COPY . .
WORKDIR "/src/src/Notifier/FeedR.Notifier"
RUN dotnet build "FeedR.Notifier.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedR.Notifier.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedR.Notifier.dll"]
