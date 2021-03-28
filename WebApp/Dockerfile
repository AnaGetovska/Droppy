FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Green Assistant/*.csproj", "./"]
RUN dotnet restore
COPY ["./Green Assistant/", "."]
WORKDIR /src
RUN dotnet build "Green Assistant.csproj" -c Release -o /app/
RUN dotnet publish "Green Assistant.csproj" -c Release -o /app/

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=0 /app/ .
ENTRYPOINT ["dotnet", "Green Assistant.dll"]