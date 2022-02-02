#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Feeds/Quotes/FeedR.Feeds.Quotes/FeedR.Feeds.Quotes.csproj", "src/Feeds/Quotes/FeedR.Feeds.Quotes/"]
RUN dotnet restore "src/Feeds/Quotes/FeedR.Feeds.Quotes/FeedR.Feeds.Quotes.csproj"
COPY . .
WORKDIR "/src/src/Feeds/Quotes/FeedR.Feeds.Quotes"
RUN dotnet build "FeedR.Feeds.Quotes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedR.Feeds.Quotes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedR.Feeds.Quotes.dll"]
