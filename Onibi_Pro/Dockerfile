FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Onibi_Pro/Onibi_Pro.csproj", "Onibi_Pro/"]
COPY ["Onibi_Pro.Application/Onibi_Pro.Application.csproj", "Onibi_Pro.Application/"]
COPY ["Onibi_Pro.Domain/Onibi_Pro.Domain.csproj", "Onibi_Pro.Domain/"]
COPY ["Onibi_Pro.Contracts/Onibi_Pro.Contracts.csproj", "Onibi_Pro.Contracts/"]
COPY ["Onibi_Pro.Infrastructure/Onibi_Pro.Infrastructure.csproj", "Onibi_Pro.Infrastructure/"]
COPY ["Onibi_Pro.Shared/Onibi_Pro.Shared.csproj", "Onibi_Pro.Shared/"]
RUN dotnet restore "Onibi_Pro/Onibi_Pro.csproj"
COPY . .
WORKDIR "/src/Onibi_Pro"
RUN dotnet build "Onibi_Pro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Onibi_Pro.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Onibi_Pro.dll"]