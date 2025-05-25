# File Analysis System

Микросервисная система для загрузки `.txt` файлов, анализа их текстового содержимого (подсчёт слов, символов, абзацев) и генерации облака слов.

## 📦 Состав системы

Проект состоит из трёх независимых сервисов:

| Сервис               | Назначение                                                                 |
|----------------------|---------------------------------------------------------------------------|
| **FileStoringService** | Принимает и сохраняет `.txt` файлы в файловую систему и БД               |
| **FileAnalysisService** | Анализирует тексты: количество символов, слов, абзацев + генерирует облако слов |
| **ApiGateway**         | Централизованный доступ ко всем сервисам через единый HTTP-интерфейс     |

## 🧱 Архитектура

Система построена на принципах Clean Architecture и разделена на слои:

```
📁 src/
├── FileStoringService/          
│   ├── Core                     
│   ├── UseCases                     
│   ├── Infrastructure          
│   ├── Web                      
├── FileAnalysisService/         
│   ├── Core
│   ├── UseCases
│   ├── Infrastructure
│   ├── Web
├── ApiGateway/                 
```

Внешние HTTP-запросы обрабатываются с помощью Ocelot(ApiGateway), который маршрутизирует их к соответствующим микросервисам.  
Взаимодействие между сервисами происходит синхронно по HTTP.

## 🚀 Запуск проекта

```bash
git clone https://github.com/M1steryO/KPO_BIG_HW_2.git
docker compose build
docker compose up
```

После запуска:

Swagger UI: http://localhost:7028  (здесь и представлена вся спецификация API)

## 🛠 Используемые технологии:
- ASP.NET Core 7
- Entity Framework Core (PostgreSQL)
- Ocelot API Gateway
- Docker Compose
- WordCloud API
