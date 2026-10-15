using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestionAppLibrary.DataAccess;
using QuestionsApp.Infrastructure.MongoDb.DataAccess;
using QuestionsApp.Infrastructure.MongoDb.Init;

namespace QuestionsApp.Infrastructure.MongoDb.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddQuestionAppMongoDb(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(builder.Configuration.GetConnectionString("MongoDB")));

            builder.Services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();

                var databaseName = builder.Configuration["DatabaseName"]
                 ?? throw new InvalidOperationException("MongoDb:DatabaseName is not configured.");

                return client.GetDatabase(databaseName);
            });

            builder.Services.AddSingleton<MongoDbInitializer>();
            builder.Services.AddHostedService<MongoDbStartupService>();

            builder.Services.AddSingleton<DbConnection>();
            builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
            builder.Services.AddSingleton<IStatusData, MongoStatusData>();
            builder.Services.AddSingleton<IUserData, MongoUserData>();
            builder.Services.AddSingleton<IQuestionData, MongoQuestionData>();
            builder.Services.AddSingleton<IAnswerData, MongoAnswerData>();
        }
    }
}
