FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TODO.API/TODO.API.csproj", "TODO.API/"]
RUN dotnet restore "TODO.API/TODO.API.csproj"
COPY . .
WORKDIR "/src/TODO.API"
RUN dotnet build "TODO.API.csproj" -c Release -o /app/build
RUN dotnet publish "TODO.API.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "TODO.API.dll"]