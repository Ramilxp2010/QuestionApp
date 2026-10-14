using QuestionAppUI.Models;

namespace QuestionAppUI.Data
{
    public static class SampleData
    {
        public static List<User> SampleUsers = new List<User>()
        {
            new User(){ Name = "Ramil", UserName = "Ramil", ObjectIdentifier = "ramil_123", DisplayName="Ramil", Role = "User", Email = "ramil@example.com" },
            new User(){ Name = "Tom", UserName = "Tom", ObjectIdentifier = "tom_123", DisplayName="Tom", Role = "User", Email = "tom@example.com" },
            new User(){ Name = "Kate", UserName = "Kate", ObjectIdentifier = "kate_123", DisplayName="Kate", Role = "User", Email = "kate@example.com" },
            new User(){ Name = "Admin", UserName = "Admin", ObjectIdentifier = "admin_123", DisplayName="Admin", Role = "Admin", Email = "admin@example.com" }
        };

        public static List<Category> SampleCategories = new List<Category>()
        {
            new Category { Name = "C#", Description = "Вопросы по языку С#"},
            new Category { Name = ".NET/Core", Description = "Вопросы по платформе .NET/Core"},
            new Category { Name = "ООП", Description = "Вопросы по ООП"},
            new Category { Name = "Базы данных", Description = "Вопросы по Базам данных"},
            new Category { Name = "Web-разработка", Description = "Вопросы по Web-разработке"},
            new Category { Name = "Тестирование", Description = "Вопросы по тестированию "},
            new Category { Name = "Disign Patterns", Description = "Вопросы по паттернам программирования"},
            new Category { Name = "Архитектурные шаблоны", Description = "Вопросы по архитектурным шаблонам"},
            new Category { Name = "Дополнительные темы", Description = "Вопросы по Docker, Git, Azure и тд"}
        };

        public static List<Status> SampleStatuses = new List<Status>()
        {
            new Status { Name = "Активный", Description = "Активные вопросы"},
            new Status { Name = "Архив", Description = "Архивированные вопросы"},
            new Status { Name = "Отклонен", Description = "Отклоненные вопросы"},
            
            new Status { Name = "completed", Description = "completed"},
            new Status { Name = "watching", Description = "watching"},
            new Status { Name = "upcoming", Description = "upcoming"},
            new Status { Name = "dismissed", Description = "dismissed"}
        };

        public static List<Question> SampleQuestions = new List<Question>
        {
            new Question
            {
                QuestionText = "Для чего нужен оператор checked?",
                Answer = "При применении к операции преобразования типов оператор checked позволяет сгенерировать исключение при переполнении разрядности типа.",
                DateCreated = DateTime.Now,
                Categories = new List<Category> { SampleCategories.FirstOrDefault(x => x.Name == "C#"), SampleCategories.FirstOrDefault(x => x.Name == ".NET/Core") },
                Author = new BasicUser(SampleUsers[0]),
                IsApproved = true,
            },
            new Question
            {
                QuestionText = "Что такое .NET framework ?",
                Answer = ".NET Framework - это программная платформа, разработанная корпорацией Microsoft, предназначенная для создания и выполнения приложений под Windows. Она была введена в 2002 году и стала одной из основных платформ для разработки приложений для операционных систем Windows.",
                DateCreated = DateTime.Now,
                Categories = new List<Category> {SampleCategories.FirstOrDefault(x => x.Name == ".NET/Core") },
                Author = new BasicUser(SampleUsers[0]),
                IsApproved = true,
            },
            new Question
            {
                QuestionText = "Что такое Value Type и Reference Type? Назовите различия между ними.",
                Answer = "В языках программирования, включая C#, C++, Java и другие, данные переменные могут быть разделены на две основные категории: типы значений (Value Types) и ссылочные типы (Reference Types). Эти категории определяют, как данные хранятся в памяти и как работает присваивание переменных.",
                DateCreated = DateTime.Now,
                Categories = new List < Category > 
                { 
                    SampleCategories.FirstOrDefault(x => x.Name == "C#"), 
                    SampleCategories.FirstOrDefault(x => x.Name == "ООП") ,
                    SampleCategories.FirstOrDefault(x => x.Name == "Тестирование") },
                Author = new BasicUser(SampleUsers[0]),
                IsApproved = true,
            },
            new Question
            {
                QuestionText = "Ваше понимание упаковки (Boxing) и распаковки (Unboxing).",
                Answer = "Ссылочные типы в языке C# определяются с помощью ключевого слова class. ссылочные типы – это все типы, кроме прямых или косвенных потомков System.ValueType. Массивы интерфейсы и делегаты – это тоже ссылочные тип. Типы-значения в языке C# – это типы, определённые с помощью ключевых слов struct, enum, а также все фундаментальные кроме string. Переменные ссылочных типов содержат адреса объектов, а переменные типов-значений содержат сами объекты. ",
                DateCreated = DateTime.Now,
                Categories = new List<Category> {SampleCategories.FirstOrDefault(x => x.Name == "C#") },
                Author = new BasicUser(SampleUsers[0]),
                IsApproved = true,
            },
            new Question
            {
                QuestionText = "Может ли значение типа DateTime принимать значение null?",
                Answer = "Нет, если только не DateTime?",
                DateCreated = DateTime.Now,
                Categories = new List < Category > { SampleCategories.FirstOrDefault(x => x.Name == "C#") },
                Author = new BasicUser(SampleUsers[0]),
                IsApproved = true,
            },
            new Question
            {
                QuestionText = "Ваше понимание работы класса StringBuilder. Есть ли преимущество перед использованием String?",
                Answer = "да это динамическая строка и при изменении её элемента меняется только он, а не занова создается объект как в случае со String",
                DateCreated = DateTime.Now,
                Categories = new List < Category > { SampleCategories.FirstOrDefault(x => x.Name == "C#") },
                Author = new BasicUser(SampleUsers[0]),
                IsApproved = true,
            },
        };

        public static List<Answer> SampleAnswers = new List<Answer>
        {
            new Answer
            {
                AnswerText = "Ссылочные типы в языке C# определяются с помощью ключевого слова class. ссылочные типы – это все типы, кроме прямых или косвенных потомков System.ValueType. Массивы интерфейсы и делегаты – это тоже ссылочные тип. Типы-значения в языке C# – это типы, определённые с помощью ключевых слов struct, enum, а также все фундаментальные кроме string. Переменные ссылочных типов содержат адреса объектов, а переменные типов-значений содержат сами объекты. ",
                Author = SampleUsers[0]
            },
            new Answer
            {
                AnswerText = "Ссылочные типы в языке C# определяются с помощью ключевого слова class.",
                Author = SampleUsers[1]
            },
            new Answer
            {
                AnswerText = "Ссылочные типы в языке C# определяются с помощью ключевого слова class.",
                Author = SampleUsers[2]
            }
        };
    }
}
