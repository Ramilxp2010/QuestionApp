using Microsoft.Extensions.Hosting;

namespace QuestionsApp.Infrastructure.MongoDb.Init
{
    internal class MongoDbStartupService : IHostedService
    {
        private readonly MongoDbInitializer _mongoDbInitializer;

        public MongoDbStartupService(MongoDbInitializer mongoDbInitializer)
        {
            _mongoDbInitializer = mongoDbInitializer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _mongoDbInitializer.InitializeAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
