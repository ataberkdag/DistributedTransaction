#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/Order/Order.API/Order.API.csproj", "src/services/Order/Order.API/"]
COPY ["src/services/Order/Order.Infrastructure/Order.Infrastructure.csproj", "src/services/Order/Order.Infrastructure/"]
COPY ["src/core/Core.Infrastructure/Core.Infrastructure.csproj", "src/core/Core.Infrastructure/"]
COPY ["src/core/Core.Application/Core.Application.csproj", "src/core/Core.Application/"]
COPY ["src/core/Core.Domain/Core.Domain.csproj", "src/core/Core.Domain/"]
COPY ["src/services/Order/Order.Application/Order.Application.csproj", "src/services/Order/Order.Application/"]
COPY ["src/services/Order/Order.Domain/Order.Domain.csproj", "src/services/Order/Order.Domain/"]
RUN dotnet restore "src/services/Order/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/src/services/Order/Order.API"
RUN dotnet build "Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]