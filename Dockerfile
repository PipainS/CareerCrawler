# Этап сборки: используем официальный .NET SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Копируем все файлы решения и проектов
COPY . .

# Публикуем проект, явно указывая путь к файлу проекта
RUN dotnet publish CareerCrawler/CareerCrawler.csproj -c Release -o /app

# Этап выполнения: используем легковесный образ .NET Runtime 8.0
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Копируем собранное приложение из этапа сборки
COPY --from=build /app .

# Точка входа: запускаем приложение (убедитесь, что имя dll совпадает)
ENTRYPOINT ["dotnet", "CareerCrawler.dll"]
