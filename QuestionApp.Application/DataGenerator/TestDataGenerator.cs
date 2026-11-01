﻿using QuestionApp.Application.DataGenerator.Data;
using QuestionApp.Application.DataGenerator.Model;
using QuestionApp.Application.DataGenerator.Options;
using QuestionAppLibrary.Models;
using QuestionModel = QuestionAppLibrary.Models.Question;

namespace QuestionApp.Application.DataGenerator;

/// <summary>
/// Детерминированный генератор тестовых данных.
/// Один и тот же <see cref="SeedOptions.RandomSeed"/> всегда даёт один и тот же набор:
/// одинаковые ID, тексты, даты и голоса.
/// </summary>
/// <remarks>
/// Данные из <see cref="SampleData"/> всегда входят в результат и идут первыми.
/// Остальное добирается генерацией до UserCount, QuestionCount и AnswerCount.
/// Если число меньше количества образцов, возвращаются только образцы.
/// Пароли не хешируются: PasswordHash остаётся null, его заполняет сидер
/// через IPasswordHasher, как это делает DemoUserSeeder.
/// </remarks>
public sealed class TestDataGenerator
{
    // Фиксированная точка отсчёта вместо DateTime.Now, чтобы данные были воспроизводимыми.
    private static readonly DateTime BaseDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private const int HistoryDays = 365;
    private const int MaxVotesPerItem = 50;

    private static readonly string[] MaleFirstNames =
        ["Ivan", "Petr", "Dmitry", "Sergey", "Alexey", "Artem", "Nikita", "Timur", "Ruslan", "Egor"];

    private static readonly string[] FemaleFirstNames =
        ["Anna", "Olga", "Elena", "Maria", "Natalia", "Irina", "Daria", "Alina", "Kamila", "Polina"];

    private static readonly string[] LastNames =
    [
        "Ivanov", "Petrov", "Sidorov", "Smirnov", "Kuznetsov", "Popov", "Volkov", "Sokolov",
        "Lebedev", "Kozlov", "Novikov", "Morozov", "Pavlov", "Fedorov", "Khabibullin", "Galiev"
    ];

    // Темы по имени категории из SampleData. Неизвестные категории получают DefaultTopics.
    private static readonly Dictionary<string, string[]> TopicsByCategory = new()
    {
        ["C#"] = ["async/await", "LINQ", "делегаты", "события", "обобщения (generics)", "records", "pattern matching", "Span<T>", "nullable reference types", "yield return"],
        [".NET/Core"] = ["сборщик мусора (GC)", "CLR", "Dependency Injection", "middleware", "конфигурация (IOptions)", "Kestrel", "IHostedService", "логирование (ILogger)"],
        ["ООП"] = ["инкапсуляция", "наследование", "полиморфизм", "абстракция", "интерфейсы", "абстрактные классы", "SOLID", "композиция"],
        ["Базы данных"] = ["индексы", "транзакции", "уровни изоляции", "нормализация", "EF Core", "N+1 запросы", "MongoDB", "шардирование"],
        ["Web-разработка"] = ["REST", "CORS", "JWT", "cookie-аутентификация", "HTTP-кэширование", "Blazor Server", "SignalR", "gRPC"],
        ["Тестирование"] = ["unit-тесты", "моки (Moq)", "интеграционные тесты", "TDD", "xUnit", "покрытие кода", "WebApplicationFactory"],
        ["Disign Patterns"] = ["Singleton", "Factory Method", "Strategy", "Decorator", "Observer", "Adapter", "Builder", "Mediator"],
        ["Архитектурные шаблоны"] = ["Clean Architecture", "CQRS", "Event Sourcing", "микросервисы", "монолит", "DDD", "Onion Architecture", "Outbox"],
        ["Дополнительные темы"] = ["Docker", "Docker Compose", "Git rebase", "Git merge", "Azure", "CI/CD", "Kubernetes", "Nginx"]
    };

    private static readonly string[] DefaultTopics = ["отладка", "профилирование", "code review", "рефакторинг"];

    private static readonly string[] SingleTopicTemplates =
    [
        "Что такое {0}?",
        "{0}: как это работает?",
        "{0}: когда стоит применять?",
        "{0}: плюсы и минусы",
        "{0}: типичные ошибки и как их избежать",
        "Объясните тему «{0}» на простом примере",
        "Что нужно знать про «{0}» на собеседовании?"
    ];

    private static readonly string[] CompareTemplates =
    [
        "Чем отличаются {0} и {1}?",
        "{0} или {1}: что выбрать и почему?"
    ];

    private static readonly string[] AnswerOpenings =
    [
        "Если коротко, {0} решает конкретную задачу, и важно понимать её границы.",
        "{0} стоит рассматривать прежде всего с точки зрения сопровождаемости кода.",
        "На практике {0} чаще всего встречается в продуктовом коде и на собеседованиях.",
        "Главное про {0}: понимать, что происходит под капотом."
    ];

    private static readonly string[] AnswerDetails =
    [
        "Хорошо помогает небольшой пример и тест, который фиксирует поведение.",
        "Типичная ошибка — применять подход везде, не оценивая стоимость.",
        "Стоит посмотреть официальную документацию Microsoft, там есть хорошие примеры.",
        "В реальном проекте это обычно сочетается с логированием и метриками.",
        "Обратите внимание на производительность и аллокации в горячем пути.",
        "Полезно сравнить с альтернативами и понять компромиссы."
    ];

    // Статусы, которые UI раскрашивает (см. Index.razor.cs, GetQuestionStatusClass).
    private static readonly string[] WorkflowStatusNames = ["watching", "upcoming", "completed", "dismissed"];

    public SeedData Generate(SeedOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentOutOfRangeException.ThrowIfNegative(options.UserCount, nameof(options.UserCount));
        ArgumentOutOfRangeException.ThrowIfNegative(options.QuestionCount, nameof(options.QuestionCount));
        ArgumentOutOfRangeException.ThrowIfNegative(options.AnswerCount, nameof(options.AnswerCount));

        var random = new Random(options.RandomSeed);
        var ids = new ObjectIdFactory(random);

        // Копируем образцы: SampleData — статические списки, их используют
        // другие сидеры, поэтому присваивать им Id напрямую нельзя.
        var categories = SampleData.SampleCategories
            .Select(c => new Category { Id = ids.Next(BaseDate), Name = c.Name, Description = c.Description })
            .ToList();

        var statuses = SampleData.SampleStatuses
            .Select(s => new Status { Id = ids.Next(BaseDate), Name = s.Name, Description = s.Description })
            .ToList();

        var users = GenerateUsers(options.UserCount, random, ids);
        var questions = GenerateQuestions(options.QuestionCount, users, categories, statuses, random, ids);
        var answers = GenerateAnswers(options.AnswerCount, users, questions, random, ids);

        return new SeedData
        {
            Users = users,
            Categories = categories,
            Statuses = statuses,
            Questions = questions,
            Answers = answers
        };
    }

    private static List<User> GenerateUsers(int count, Random random, ObjectIdFactory ids)
    {
        var users = new List<User>(Math.Max(count, SampleData.SampleUsers.Count));

        foreach (var template in SampleData.SampleUsers)
        {
            users.Add(new User
            {
                Id = ids.Next(BaseDate.AddDays(-HistoryDays)),
                ObjectIdentifier = template.ObjectIdentifier,
                UserName = template.UserName,
                Name = template.Name,
                LastName = template.LastName,
                DisplayName = template.DisplayName,
                Role = template.Role,
                Email = template.Email
            });
        }

        for (var i = users.Count + 1; users.Count < count; i++)
        {
            var isFemale = random.Next(2) == 0;
            var firstName = Pick(isFemale ? FemaleFirstNames : MaleFirstNames, random);
            var lastName = Pick(LastNames, random) + (isFemale ? "a" : string.Empty);
            var userName = $"{firstName}.{lastName}{i}".ToLowerInvariant();

            users.Add(new User
            {
                Id = ids.Next(RandomDate(random)),
                ObjectIdentifier = $"test_user_{i:D5}",
                UserName = userName,
                Name = firstName,
                LastName = lastName,
                DisplayName = $"{firstName} {lastName}",
                Role = "User",
                Email = $"{userName}@example.com"
            });
        }

        return users;
    }

    private static List<QuestionModel> GenerateQuestions(
        int count,
        List<User> users,
        List<Category> categories,
        List<Status> statuses,
        Random random,
        ObjectIdFactory ids)
    {
        var questions = new List<QuestionModel>(Math.Max(count, SampleData.SampleQuestions.Count));
        var categoriesByName = categories.ToDictionary(c => c.Name);
        var usersByObjectId = users.ToDictionary(u => u.ObjectIdentifier);

        var workflowStatuses = statuses
            .Where(s => WorkflowStatusNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase))
            .ToList();
        if (workflowStatuses.Count == 0)
        {
            workflowStatuses = statuses;
        }

        var defaultStatus = workflowStatuses.FirstOrDefault(s =>
            string.Equals(s.Name, "watching", StringComparison.OrdinalIgnoreCase)) ?? workflowStatuses.FirstOrDefault();

        // 1. Образцы из SampleData: перепривязываем категории и автора к сгенерированным объектам с Id.
        foreach (var template in SampleData.SampleQuestions)
        {
            var author = template.Author?.ObjectId is { } objectId && usersByObjectId.TryGetValue(objectId, out var u)
                ? u
                : Pick(users, random);

            var date = RandomDate(random);
            var question = new QuestionModel
            {
                Id = ids.Next(date),
                QuestionText = template.QuestionText,
                Description = template.Description,
                Answer = template.Answer,
                DateCreated = date,
                Author = new BasicUser(author),
                Categories = (template.Categories ?? [])
                    .Where(c => c != null && categoriesByName.ContainsKey(c.Name))
                    .Select(c => categoriesByName[c.Name])
                    .ToList(),
                Status = defaultStatus!,
                IsApproved = true,
                UserVotes = PickVoterIds(users, author.Id, random)
            };

            questions.Add(question);
        }

        // 2. Сгенерированные вопросы.
        while (questions.Count < count)
        {
            var author = Pick(users, random);
            var questionCategories = PickDistinct(categories, random.Next(1, 4), random);
            var mainCategory = questionCategories[0];
            var topics = TopicsByCategory.GetValueOrDefault(mainCategory.Name, DefaultTopics);

            string questionText;
            string mainTopic;
            if (random.NextDouble() < 0.2 && topics.Length > 1)
            {
                var pair = PickDistinct(topics, 2, random);
                mainTopic = pair[0];
                questionText = Capitalize(string.Format(Pick(CompareTemplates, random), pair[0], pair[1]));
            }
            else
            {
                mainTopic = Pick(topics, random);
                questionText = Capitalize(string.Format(Pick(SingleTopicTemplates, random), mainTopic));
            }

            // Распределение состояний: большинство одобрено,
            // часть ждёт модерации, часть в архиве или отклонена.
            var roll = random.NextDouble();
            var isApproved = roll < 0.85;
            var isArchived = roll is >= 0.75 and < 0.85;
            var isRejected = roll >= 0.93;

            var date = RandomDate(random);
            questions.Add(new QuestionModel
            {
                Id = ids.Next(date),
                QuestionText = questionText,
                Description = $"Категория: {mainCategory.Name}. Интересует практическое применение темы «{mainTopic}».",
                Answer = random.NextDouble() < 0.7 ? BuildAnswerText(mainTopic, random) : null!,
                DateCreated = date,
                Author = new BasicUser(author),
                Categories = questionCategories,
                Status = Pick(workflowStatuses, random),
                IsApproved = isApproved,
                IsArchived = isArchived,
                IsRejected = isRejected,
                UserVotes = isApproved ? PickVoterIds(users, author.Id, random) : []
            });
        }

        // Обратные ссылки у пользователей, как их поддерживает MongoQuestionData.
        var usersById = users.ToDictionary(u => u.Id);
        foreach (var question in questions)
        {
            var basic = new BasicQuestion(question);
            usersById[question.Author.Id].AuthoredQuestions.Add(basic);
            foreach (var voterId in question.UserVotes)
            {
                usersById[voterId].VotedQuestions.Add(basic);
            }
        }

        return questions;
    }

    private static List<Answer> GenerateAnswers(
        int count,
        List<User> users,
        List<QuestionModel> questions,
        Random random,
        ObjectIdFactory ids)
    {
        var answers = new List<Answer>(Math.Max(count, SampleData.SampleAnswers.Count));

        // Отвечать можно только на видимые вопросы.
        var answerable = questions.Where(q => q.IsApproved && !q.IsRejected).ToList();
        if (answerable.Count == 0)
        {
            return answers;
        }

        var usersByObjectId = users.ToDictionary(u => u.ObjectIdentifier);
        var sampleQuestionCount = Math.Min(SampleData.SampleQuestions.Count, answerable.Count);

        // 1. Образцы ответов распределяем по вопросам-образцам по кругу.
        for (var i = 0; i < SampleData.SampleAnswers.Count; i++)
        {
            var template = SampleData.SampleAnswers[i];
            var question = answerable[i % sampleQuestionCount];
            var author = template.Author?.ObjectIdentifier is { } objectId && usersByObjectId.TryGetValue(objectId, out var u)
                ? u
                : Pick(users, random);

            answers.Add(CreateAnswer(template.AnswerText, question, author, isApproved: true, users, random, ids));
        }

        // 2. Сгенерированные ответы.
        while (answers.Count < count)
        {
            var question = Pick(answerable, random);
            var author = PickOther(users, question.Author.Id, random);
            var topic = question.Categories.Count > 0
                ? Pick(TopicsByCategory.GetValueOrDefault(question.Categories[0].Name, DefaultTopics), random)
                : Pick(DefaultTopics, random);

            answers.Add(CreateAnswer(BuildAnswerText(topic, random), question, author,
                isApproved: random.NextDouble() < 0.9, users, random, ids));
        }

        var usersById = users.ToDictionary(u => u.Id);
        foreach (var answer in answers)
        {
            var basic = new BasicAnswer(answer);
            usersById[answer.Author.Id].AuthoredAnswers.Add(basic);
            foreach (var voterId in answer.Votes)
            {
                usersById[voterId].VotedAnswers.Add(basic);
            }
        }

        return answers;
    }

    private static Answer CreateAnswer(
        string text,
        QuestionModel question,
        User author,
        bool isApproved,
        List<User> users,
        Random random,
        ObjectIdFactory ids)
    {
        // Ответ появляется после вопроса, но не позже BaseDate.
        var maxDelay = (BaseDate - question.DateCreated).TotalMinutes;
        var date = question.DateCreated.AddMinutes(random.NextDouble() * Math.Min(maxDelay, 30 * 24 * 60));

        return new Answer
        {
            Id = ids.Next(date),
            AnswerText = text,
            Author = CreateAuthorSnapshot(author),
            CreateDate = date,
            QuestionId = question.Id,
            IsApproved = isApproved,
            Votes = isApproved ? PickVoterIds(users, author.Id, random, maxVotes: 15) : []
        };
    }

    /// <summary>
    /// Answer.Author — полный User, и в Mongo он встраивается в документ ответа целиком.
    /// Кладём облегчённую копию без PasswordHash и без списков,
    /// иначе каждый ответ тянул бы хеш пароля и всю историю автора.
    /// </summary>
    private static User CreateAuthorSnapshot(User user) => new()
    {
        Id = user.Id,
        ObjectIdentifier = user.ObjectIdentifier,
        UserName = user.UserName,
        Name = user.Name,
        LastName = user.LastName,
        DisplayName = user.DisplayName,
        Email = user.Email,
        Role = user.Role
    };

    private static string BuildAnswerText(string topic, Random random)
    {
        var details = PickDistinct(AnswerDetails, random.Next(1, 3), random);
        return Capitalize(string.Format(Pick(AnswerOpenings, random), topic)) + " " + string.Join(" ", details);
    }

    private static string Capitalize(string text) =>
        string.IsNullOrEmpty(text) ? text : char.ToUpperInvariant(text[0]) + text[1..];

    /// <summary>
    /// Голоса с «длинным хвостом»: у большинства записей мало голосов, у немногих много.
    /// В голосах хранится User.Id, так же как в MongoQuestionData.UpVoted.
    /// </summary>
    private static HashSet<string> PickVoterIds(List<User> users, string excludeUserId, Random random, int maxVotes = MaxVotesPerItem)
    {
        var max = Math.Min(maxVotes, users.Count - 1);
        if (max <= 0)
        {
            return [];
        }

        var votes = (int)Math.Round(Math.Pow(random.NextDouble(), 3) * max);
        var result = new HashSet<string>();
        while (result.Count < votes)
        {
            var voter = Pick(users, random);
            if (voter.Id != excludeUserId)
            {
                result.Add(voter.Id);
            }
        }

        return result;
    }

    private static DateTime RandomDate(Random random) =>
        BaseDate.AddMinutes(-random.NextDouble() * HistoryDays * 24 * 60);

    private static T Pick<T>(IReadOnlyList<T> items, Random random) => items[random.Next(items.Count)];

    private static User PickOther(List<User> users, string excludeUserId, Random random)
    {
        if (users.Count == 1)
        {
            return users[0];
        }

        User user;
        do
        {
            user = Pick(users, random);
        } while (user.Id == excludeUserId);

        return user;
    }

    /// <summary>Частичная перетасовка Фишера-Йетса: count различных элементов.</summary>
    private static List<T> PickDistinct<T>(IReadOnlyList<T> items, int count, Random random)
    {
        var buffer = items.ToArray();
        count = Math.Min(count, buffer.Length);
        for (var i = 0; i < count; i++)
        {
            var j = random.Next(i, buffer.Length);
            (buffer[i], buffer[j]) = (buffer[j], buffer[i]);
        }

        return buffer.Take(count).ToList();
    }

    /// <summary>
    /// Генерирует валидные 24-символьные ObjectId (их ждёт [BsonRepresentation(BsonType.ObjectId)]),
    /// не завися от драйвера MongoDB: 4 байта времени + 8 байт из seeded Random.
    /// </summary>
    private sealed class ObjectIdFactory(Random random)
    {
        private readonly HashSet<string> _issued = [];
        private readonly byte[] _buffer = new byte[12];

        public string Next(DateTime timestamp)
        {
            var seconds = (uint)Math.Max(0, new DateTimeOffset(timestamp.ToUniversalTime()).ToUnixTimeSeconds());
            _buffer[0] = (byte)(seconds >> 24);
            _buffer[1] = (byte)(seconds >> 16);
            _buffer[2] = (byte)(seconds >> 8);
            _buffer[3] = (byte)seconds;

            string id;
            do
            {
                random.NextBytes(_buffer.AsSpan(4));
                id = Convert.ToHexStringLower(_buffer);
            } while (!_issued.Add(id));

            return id;
        }
    }
}
