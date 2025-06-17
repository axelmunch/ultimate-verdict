FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Backend ./Backend
COPY Database ./Database
COPY Domain ./Domain

RUN dotnet restore Backend/Backend.csproj
RUN dotnet publish Backend/Backend.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
CMD ["dotnet", "Backend.dll"]
