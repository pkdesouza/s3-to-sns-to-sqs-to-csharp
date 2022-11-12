FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bemobi/Bemobi.Consumer/Bemobi.Consumer.csproj", "Bemobi/"]
RUN dotnet restore "Bemobi/Bemobi.Consumer.csproj"
COPY . .
WORKDIR "/src/Bemobi/Bemobi.Consumer/"
RUN dotnet build "Bemobi.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bemobi.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bemobi.Consumer.dll"]
