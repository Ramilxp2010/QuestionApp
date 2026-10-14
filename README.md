# QuestionApp

Blazor Server приложение для вопросов и ответов (Q&A) с хранением данных в MongoDB.

Пользователи могут просматривать одобренные вопросы, предлагать новые, отвечать и голосовать. Администратор модерирует очередь и меняет статусы вопросов.

## Требования

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (в репозитории закреплён SDK `10.0.301` через [`global.json`](global.json))
- Локальный MongoDB на `localhost:27017`

## Быстрый старт

```powershell
dotnet restore
dotnet run --project QuestionAppUI
```

Приложение слушает:

- HTTPS: https://localhost:7123
- HTTP: http://localhost:5172

Строка подключения для Development задана в [`QuestionAppUI/appsettings.Development.json`](QuestionAppUI/appsettings.Development.json):

```json
"ConnectionStrings": {
  "MongoDB": "mongodb://localhost:27017"
}
```

База данных: `QuestionApp`.

### Пользователи и начальные данные

При первом запуске с пустой Mongo автоматически создаются:

- демо-пользователи (пароль как hash через `PasswordHasher`)
- категории, статусы, вопросы и ответы

Повторный ручной seed (только пустые коллекции): Admin → `/SampleData`.

### Демо-логины

| Пользователь | Пароль | Роль  |
|--------------|--------|-------|
| Ramil        | 1234   | User  |
| Tom          | 1234   | User  |
| Kate         | 1234   | User  |
| Admin        | 1234   | Admin |

Auth: Mongo `UserModel` + cookie. Загрузка картинок из редактора: `POST /upload/image` → `wwwroot/uploads/`.

## Структура решения

| Проект | Назначение |
|--------|------------|
| [`QuestionAppUI`](QuestionAppUI/) | Blazor Server host: UI, cookie-auth, DI |
| [`QuestionAppLibrary`](QuestionAppLibrary/) | Модели и доступ к MongoDB |

## Документация

- [Технологическая карта](docs/TECHNOLOGY_MAP.md) — стек, версии, зависимости
- [Архитектура](docs/ARCHITECTURE.md) — слои, auth, маршруты, данные, ограничения
- [План перехода на стек вакансий](docs/STACK_TRANSITION_PLAN.md) — пошаговый rewrite на Web API + EF + PostgreSQL + Angular
- [План обучения Angular](docs/ANGULAR_LEARNING_PLAN.md) — дневной маршрут JS/TS/Angular для .NET-разработчика (UI этапы 6–8)
