# Базовый образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY TraneServer/*.csproj ./TraneServer/
RUN dotnet restore "TraneServer/TraneServer.csproj"


COPY . .
WORKDIR /src/TraneServer
RUN dotnet publish "TraneServer.csproj" -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TraneServer.dll"]