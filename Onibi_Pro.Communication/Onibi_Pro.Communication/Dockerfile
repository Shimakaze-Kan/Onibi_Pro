#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Onibi_Pro.Communication/Onibi_Pro.Communication.csproj", "Onibi_Pro.Communication/"]
RUN dotnet restore "./Onibi_Pro.Communication/./Onibi_Pro.Communication.csproj"
COPY . .
WORKDIR "/src/Onibi_Pro.Communication"
RUN dotnet build "./Onibi_Pro.Communication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Onibi_Pro.Communication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Onibi_Pro.Communication.dll"]