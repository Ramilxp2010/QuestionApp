using QuestionAppUI.Models;

namespace QuestionAppUI.Data
{
    public static class SampleData
    {
        public static List<AuthUserModel> AuthUsers = new List<AuthUserModel>()
        {
            new AuthUserModel(){ UserName = "Ramil", Identifier = "ramil_123", Password="1234", Role= "User"},
            new AuthUserModel(){ UserName = "Tom", Identifier = "tom_123", Password="1234", Role= "User"},
            new AuthUserModel(){ UserName = "Kate", Identifier = "kate_123", Password="1234", Role= "User"},
            new AuthUserModel(){ UserName = "Admin", Identifier = "admin_123", Password="1234", Role= "Admin"}
        };
        
        public static List<UserModel> SampleUsers = new List<UserModel>()
        {
            new UserModel(){ Name = "Ramil", ObjectIdentifier = "ramil_123", DisplayName="Ramil" },
            new UserModel(){ Name = "Tom", ObjectIdentifier = "tom_123", DisplayName="Tom" },
            new UserModel(){ Name = "Kate", ObjectIdentifier = "kate_123", DisplayName="Kate" },
            new UserModel(){ Name = "Admin", ObjectIdentifier = "admin_123", DisplayName="Admin" }
        };

        public static List<CategoryModel> SampleCategories = new List<CategoryModel>()
        {
            new CategoryModel { Name = "C#", Description = "Вопросы по языку С#"},
            new CategoryModel { Name = ".NET/Core", Description = "Вопросы по платформе .NET/Core"},
            new CategoryModel { Name = "ООП", Description = "Вопросы по ООП"},
            new CategoryModel { Name = "Базы данных", Description = "Вопросы по Базам данных"},
            new CategoryModel { Name = "Web-разработка", Description = "Вопросы по Web-разработке"},
            new CategoryModel { Name = "Тестирование", Description = "Вопросы по тестированию "},
            new CategoryModel { Name = "Disign Patterns", Description = "Вопросы по паттернам программирования"},
            new CategoryModel { Name = "Архитектурные шаблоны", Description = "Вопросы по архитектурным шаблонам"},
            new CategoryModel { Name = "Дополнительные темы", Description = "Вопросы по Docker, Git, Azure и тд"}
        };

        public static List<StatusModel> SampleStatuses = new List<StatusModel>()
        {
            new StatusModel { Name = "Активный", Description = "Активные вопросы"},
            new StatusModel { Name = "Архив", Description = "Архивированные вопросы"},
            new StatusModel { Name = "Отклонен", Description = "Отклоненные вопросы"},
            
            new StatusModel { Name = "completed", Description = "completed"},
            new StatusModel { Name = "watching", Description = "watching"},
            new StatusModel { Name = "upcoming", Description = "upcoming"},
            new StatusModel { Name = "dismissed", Description = "dismissed"}
        };

        public static List<QuestionModel> SampleQuestions = new List<QuestionModel>
        {
            new QuestionModel
            {
                Question = "Для чего нужен оператор checked?",
                Answer = "При применении к операции преобразования типов оператор checked позволяет сгенерировать исключение при переполнении разрядности типа.",
                DateCreated = DateTime.Now,
                Category = SampleCategories.FirstOrDefault(x => x.Name == "C#"),
                Author = new BasicUserModel(SampleUsers[0]),
                IsAproved = true,
            },
            new QuestionModel
            {
                Question = "Что такое .NET framework ?",
                Answer = ".NET Framework - это программная платформа, разработанная корпорацией Microsoft, предназначенная для создания и выполнения приложений под Windows. Она была введена в 2002 году и стала одной из основных платформ для разработки приложений для операционных систем Windows.",
                DateCreated = DateTime.Now,
                Category = SampleCategories.FirstOrDefault(x => x.Name == ".NET/Core"),
                Author = new BasicUserModel(SampleUsers[0]),
                IsAproved = true,
            },
            new QuestionModel
            {
                Question = "Что такое Value Type и Reference Type? Назовите различия между ними.",
                Answer = "В языках программирования, включая C#, C++, Java и другие, данные переменные могут быть разделены на две основные категории: типы значений (Value Types) и ссылочные типы (Reference Types). Эти категории определяют, как данные хранятся в памяти и как работает присваивание переменных.",
                DateCreated = DateTime.Now,
                Category = SampleCategories.FirstOrDefault(x => x.Name == "C#"),
                Author = new BasicUserModel(SampleUsers[0]),
                IsAproved = true,
            },
            new QuestionModel
            {
                Question = "Ваше понимание упаковки (Boxing) и распаковки (Unboxing).",
                Answer = "Ссылочные типы в языке C# определяются с помощью ключевого слова class. ссылочные типы – это все типы, кроме прямых или косвенных потомков System.ValueType. Массивы интерфейсы и делегаты – это тоже ссылочные тип. Типы-значения в языке C# – это типы, определённые с помощью ключевых слов struct, enum, а также все фундаментальные кроме string. Переменные ссылочных типов содержат адреса объектов, а переменные типов-значений содержат сами объекты. ",
                DateCreated = DateTime.Now,
                Category = SampleCategories.FirstOrDefault(x => x.Name == "C#"),
                Author = new BasicUserModel(SampleUsers[0]),
                IsAproved = true,
            },
            new QuestionModel
            {
                Question = "Может ли значение типа DateTime принимать значение null?",
                Answer = "Нет, если только не DateTime?",
                DateCreated = DateTime.Now,
                Category = SampleCategories.FirstOrDefault(x => x.Name == "C#"),
                Author = new BasicUserModel(SampleUsers[0]),
                IsAproved = true,
            },
            new QuestionModel
            {
                Question = "Ваше понимание работы класса StringBuilder. Есть ли преимущество перед использованием String?",
                Answer = "да это динамическая строка и при изменении её элемента меняется только он, а не занова создается объект как в случае со String",
                DateCreated = DateTime.Now,
                Category = SampleCategories.FirstOrDefault(x => x.Name == "C#"),
                Author = new BasicUserModel(SampleUsers[0]),
                IsAproved = true,
            },
        };
    }
}
