#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OutlayManagerPortable.API/OutlayManagerPortable.API.csproj", "OutlayManagerPortable.API/"]
COPY ["OutlayManagerPortable.DTO/OutlayManagerPortable.DTO.csproj", "OutlayManagerPortable.DTO/"]
COPY ["OutlayManagerPortable.Infraestructure/OutlayManagerPortable.Infraestructure.csproj", "OutlayManagerPortable.Infraestructure/"]
RUN dotnet restore "OutlayManagerPortable.API/OutlayManagerPortable.API.csproj"
COPY . .
WORKDIR "/src/OutlayManagerPortable.API"
RUN dotnet build "OutlayManagerPortable.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OutlayManagerPortable.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OutlayManagerPortable.API.dll"]