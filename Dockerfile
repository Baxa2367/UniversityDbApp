
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["UniversityDbApp.csproj", "./"]

RUN dotnet restore "UniversityDbApp.csproj"

COPY . .

RUN dotnet tool install --global dotnet-ef --version 8.0.0

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet ef migrations add InitialDb --context AppDbContext --output-dir Data/Migrations

RUN dotnet publish "UniversityDbApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UniversityDbApp.dll"]
